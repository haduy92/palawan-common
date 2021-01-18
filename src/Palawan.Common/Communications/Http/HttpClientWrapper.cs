using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Palawan.Common.Extensions;
using Newtonsoft.Json;
using Serilog;

namespace Palawan.Common.Communications.Http
{
	public class HttpClientWrapper : IHttpClientWrapper
	{
		private readonly HttpClient _httpClient;
		private readonly ILogger _logger;

		private static readonly JsonSerializerSettings IgnoreNullJsonSerializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore
		};

		private static readonly JsonSerializerSettings IncludeNullJsonSerializerSettings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Include
		};

		public HttpClientWrapper(HttpClient httpClient, ILogger logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		#region GET

		public async Task<HttpResponseMessage> GetAsync(string uri
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Get, uri, authConfig, headers: headers);
			return await SendAsync(request, cancellationToken ?? CancellationToken.None);
		}

		public async Task<TRequest> GetAsync<TRequest>(string uri
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Get, uri, authConfig, headers: headers);
			var response = await SendAsync(request, cancellationToken ?? CancellationToken.None, HttpCompletionOption.ResponseHeadersRead);

			return JsonConvert.DeserializeObject<TRequest>(await response.Content.ReadAsStringAsync());
		}

		public async Task<Stream> GetStreamAsync(string uri
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Get, uri, authConfig, headers: headers);
			var response = await SendAsync(request, cancellationToken ?? CancellationToken.None, HttpCompletionOption.ResponseHeadersRead);

			return await response.Content.ReadAsStreamAsync();
		}

		#endregion

		#region POST

		public async Task<HttpResponseMessage> PostAsync(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Post, uri, authConfig, headers: headers);
			return await SendAsync(request, cancellationToken ?? CancellationToken.None);
		}

		public async Task<TResponse> PostAsync<TResponse>(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var response = await PostAsync(uri, requestContent, authConfig, headers, cancellationToken);
			return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
		}

		public async Task<HttpResponseMessage> PostAsJsonAsync<TRequest>(string uri
			, TRequest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PostAsync(uri, content, authConfig, headers, cancellationToken);
		}

		public async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(string uri
			, TRequest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PostAsync<TResponse>(uri, content, authConfig, headers, cancellationToken);
		}

		public async Task<TResponse> PostAsXmlAsync<TRequest, TResponse>(string uri
			, TRequest bodies
			, string soapAction = null
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken cancellationToken = default(CancellationToken))
		{
			var request = CreateXmlRequest(uri, bodies, soapAction, authConfig, headers);
			var response = await SendAsync(request, cancellationToken);
			var stream = await response.Content.ReadAsStreamAsync();

			using (var streamReader = new StreamReader(stream))
			{
				var soapResponse = XDocument.Load(streamReader);
				var xmlAttribute = Attribute.GetCustomAttribute(typeof(TResponse), typeof(XmlRootAttribute)) as XmlRootAttribute;

				XNamespace ns = xmlAttribute.Namespace;
				var xml = soapResponse.Descendants(ns + xmlAttribute.ElementName).FirstOrDefault()?.ToString();

				return xml.FromXML<TResponse>();
			}
		}

		public async Task<ICollection<HttpContent>> PostAsXmlAsync<TRequest>(string uri
			, TRequest bodies
			, string soapAction = null
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken cancellationToken = default(CancellationToken))
		{
			var request = CreateXmlRequest(uri, bodies, soapAction, authConfig, headers);
			var response = await SendAsync(request, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception();
			}

			var provider = await response.Content.ReadAsMultipartAsync();

			return provider.Contents;
		}

		#endregion

		#region DELETE

		public async Task<HttpResponseMessage> DeleteAsync(string uri
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Delete, uri, authConfig, headers: headers);
			return await SendAsync(request, cancellationToken ?? CancellationToken.None);
		}

		public async Task<TResponse> DeleteAsync<TResponse>(string uri
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var response = await DeleteAsync(uri, authConfig, headers, cancellationToken);
			return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
		}

		#endregion

		#region PUT

		public async Task<HttpResponseMessage> PutAsync(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(HttpMethod.Put, uri, authConfig, content: requestContent, headers: headers);
			return await SendAsync(request, cancellationToken ?? CancellationToken.None);
		}

		public async Task<TResponse> PutAsync<TResponse>(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var response = await PutAsync(uri, requestContent, authConfig, headers, cancellationToken);
			return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
		}

		public async Task<HttpResponseMessage> PutAsJsonAsync<TResquest>(string uri
			, TResquest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PutAsync(uri, content, authConfig, headers, cancellationToken);
		}

		public async Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(string uri
			, TRequest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PutAsync<TResponse>(uri, content, authConfig, headers, cancellationToken);
		}

		#endregion

		#region PATCH

		public async Task<HttpResponseMessage> PatchAsync(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var request = CreateJsonRequest(new HttpMethod("PATCH"), uri, authConfig, content: requestContent, headers: headers);
			return await SendAsync(request, cancellationToken ?? CancellationToken.None);
		}

		public async Task<TResponse> PatchAsync<TResponse>(string uri
			, HttpContent requestContent
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var response = await PatchAsync(uri, requestContent, authConfig, headers, cancellationToken);
			return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
		}

		public async Task<HttpResponseMessage> PatchAsJsonAsync<TResquest>(string uri
			, TResquest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PatchAsync(uri, content, authConfig, headers, cancellationToken);
		}

		public async Task<TResponse> PatchAsJsonAsync<TRequest, TResponse>(string uri
			, TRequest objectContent
			, bool ignoreNull = true
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null
			, CancellationToken? cancellationToken = null)
		{
			var content = SerializeObjectAsJson(objectContent, ignoreNull);
			return await PatchAsync<TResponse>(uri, content, authConfig, headers, cancellationToken);
		}

		#endregion

		#region Private Functions

		private StringContent SerializeObjectAsJson<TRequest>(TRequest objectContent, bool ignoreNull)
		{
			const string JsonContent = "application/json";
			var json = JsonConvert.SerializeObject(objectContent, Formatting.None, ignoreNull ?
				IgnoreNullJsonSerializerSettings : IncludeNullJsonSerializerSettings);
			return new StringContent(json, Encoding.UTF8, JsonContent);
		}

		private HttpRequestMessage CreateJsonRequest(HttpMethod method
			, string uri
			, AuthConfiguration authConfig = null
			, HttpContent content = null
			, IDictionary<string, string> headers = null)
		{
			if (uri.IsNullOrWhiteSpace())
			{
				throw new ArgumentNullException(nameof(uri));
			}

			var request = new HttpRequestMessage(method, uri);

			if (authConfig != null)
			{
				authConfig.Validate();

				if (authConfig.Scheme == AuthSchemes.Basic)
				{
					request.SetBasicAuthentication(authConfig.Username, authConfig.Password);
				}
				else if (authConfig.Scheme == AuthSchemes.BearerToken)
				{
					request.SetBearerToken(authConfig.BearerToken);
				}
			}

			if (content != null)
			{
				request.Content = content;
			}

			if (headers != null)
			{
				foreach (var header in headers)
				{
					request.Headers.Add(header.Key, header.Value);
				}
			}

			return request;
		}

		private HttpRequestMessage CreateXmlRequest<TRequest>(string uri
			, TRequest bodies
			, string soapAction = null
			, AuthConfiguration authConfig = null
			, IDictionary<string, string> headers = null)
		{
			const string soapActionHeader = "SOAPAction";
			const string xmlContent = "text/xml";

			if (uri.IsNullOrWhiteSpace())
				throw new ArgumentNullException(nameof(uri));

			if (bodies == null)
				throw new ArgumentNullException(nameof(bodies));

			XNamespace soapenv = "http://schemas.xmlsoap.org/soap/envelope/";

			// Get the envelope
			var envelope = new XElement(soapenv + "Envelope", new XAttribute(XNamespace.Xmlns + nameof(soapenv), soapenv));

			// Add headers
			if (headers != null && headers.Any())
			{
				envelope.Add(new XElement(soapenv + "Header", headers));
			}

			// Add bodies
			envelope.Add(new XElement(soapenv + "Body", bodies.ToXElement()));

			// Get HTTP request
			var request = new HttpRequestMessage()
			{
				RequestUri = new Uri(uri),
				Method = HttpMethod.Post
			};

			if (authConfig != null)
			{
				authConfig.Validate();

				if (authConfig.Scheme == AuthSchemes.Basic)
				{
					request.SetBasicAuthentication(authConfig.Username, authConfig.Password);
				}
				else if (authConfig.Scheme == AuthSchemes.BearerToken)
				{
					request.SetBearerToken(authConfig.BearerToken);
				}
			}

			request.Content = new StringContent(envelope.ToString(), Encoding.UTF8, xmlContent);
			request.Content.Headers.ContentType = new MediaTypeHeaderValue(xmlContent);
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(xmlContent));

			// Add SOAP action if any
			if (!soapAction.IsNullOrWhiteSpace())
			{
				request.Content.Headers.Add(soapActionHeader, soapAction);
			}

			return request;
		}

		private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage
			, CancellationToken cancellationToken
			, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
		{
			_logger.Information("[INTERNAL Request] {Method} {Uri} content payload: {ContentLength} bytes.",
				requestMessage.Method,
				requestMessage.RequestUri.AbsoluteUri,
				requestMessage.Content?.Headers?.ContentLength
			);

			var timer = Stopwatch.StartNew();
			var response = cancellationToken == CancellationToken.None ?
				await _httpClient.SendAsync(requestMessage, completionOption) :
				await _httpClient.SendAsync(requestMessage, completionOption, cancellationToken);
			timer.Stop();

			_logger.Information("[INTERNAL Response] {Method} {Uri} in duration {Duration}ms with status code {StatusCode} and response payload: {ContentLength} bytes.",
				response?.RequestMessage?.Method,
				response?.RequestMessage?.RequestUri?.AbsoluteUri,
				timer.ElapsedMilliseconds,
				response?.StatusCode,
				response?.Content?.Headers?.ContentLength);

			return response;
		}

		#endregion
	}
}