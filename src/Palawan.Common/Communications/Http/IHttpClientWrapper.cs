using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Palawan.Common.Communications.Http
{
	public interface IHttpClientWrapper
	{
		#region GET

		/// <summary>
		/// Send a GET request.
		/// </summary>
		/// <param name="uri">The URI to request data from.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">The headers to add to the request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> GetAsync(string uri, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a GET request, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to request data from.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">The headers to add to the request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> GetAsync<TResponse>(string uri, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a GET request and return the response as Stream.
		/// </summary>
		/// <param name="uri">The URI to request data from.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">The headers to add to the request.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response stream.</returns>
		Task<Stream> GetStreamAsync(string uri, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		#endregion


		#region POST

		/// <summary>
		/// Send a POST request.
		/// </summary>
		/// <param name="uri">The URI to post to.</param>
		/// <param name="requestContent">The content to post.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PostAsync(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a POST request, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="requestContent">The content to POST.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PostAsync<TResponse>(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a POST request with the content to be serialized as JSON.
		/// </summary>
		/// <typeparam name="TResquest">The type being sent.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="objectContent">The content to POST to.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PostAsJsonAsync<TResquest>(string uri, TResquest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a POST request with the content to be serialized as JSON, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TRequest">The type being sent.</typeparam>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="objectContent">The content to POST to.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(string uri, TRequest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a POST request with the content to be serialized as XML, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TRequest">The type being sent.</typeparam>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="bodies">The body content.</param>
		/// <param name="soapAction">The SOAPAction value.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PostAsXmlAsync<TRequest, TResponse>(string uri, TRequest bodies, string soapAction = null, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Send a POST request with the content to be serialized as XML and expect to receive a multipart response.
		/// </summary>
		/// <typeparam name="T">The type being sent.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="bodies">The body content.</param>
		/// <param name="soapAction">The SOAPAction value.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The multipart response content.</returns>
		Task<ICollection<HttpContent>> PostAsXmlAsync<T>(string uri, T bodies, string soapAction = null, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken cancellationToken = default(CancellationToken));

		#endregion


		#region DELETE

		/// <summary>
		/// Send a DELETE request.
		/// </summary>
		/// <param name="uri">The URI to DELETE.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> DeleteAsync(string uri, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a DELETE request, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to POST to.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> DeleteAsync<TResponse>(string uri, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		#endregion


		#region PUT

		/// <summary>
		/// Send a PUT request.
		/// </summary>
		/// <param name="uri">The URI to PUT to.</param>
		/// <param name="requestContent">The content to PUT.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PutAsync(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PUT request, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to PUT to.</param>
		/// <param name="requestContent">The content to PUT.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PutAsync<TResponse>(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PUT request with the content to be serialized as JSON.
		/// </summary>
		/// <typeparam name="TRequest">The type being sent.</typeparam>
		/// <param name="uri">The URI to PUT to.</param>
		/// <param name="objectContent">The content to PUT.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PutAsJsonAsync<TRequest>(string uri, TRequest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PUT request with the content to be serialized as JSON, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TRequest">The type being sent.</typeparam>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to PUT to.</param>
		/// <param name="objectContent">The content to PUT to.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(string uri, TRequest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		#endregion


		#region PATCH

		/// <summary>
		/// Send a PATCH request.
		/// </summary>
		/// <param name="uri">The URI to PATCH to.</param>
		/// <param name="requestContent">The content to PATCH.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PatchAsync(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PATCH request, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to PATCH to.</param>
		/// <param name="requestContent">The content to PATCH.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PatchAsync<TResponse>(string uri, HttpContent requestContent, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PATCH request with the content to be serialized as JSON.
		/// </summary>
		/// <typeparam name="TResquest">The type being sent.</typeparam>
		/// <param name="uri">The URI to PATCH to.</param>
		/// <param name="objectContent">The content to PATCH.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The HTTP response messsage.</returns>
		Task<HttpResponseMessage> PatchAsJsonAsync<TResquest>(string uri, TResquest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		/// <summary>
		/// Send a PATCH request with the content to be serialized as JSON, then deserialize the response to a specified type.
		/// </summary>
		/// <typeparam name="TRequest">The type being sent.</typeparam>
		/// <typeparam name="TResponse">The type being returned.</typeparam>
		/// <param name="uri">The URI to PATCH to.</param>
		/// <param name="objectContent">The content to PATCH.</param>
		/// <param name="ignoreNull">Whether to include null value fields in the serialized JSON.</param>
		/// <param name="authConfig">Authentication configuration (Basic or Bearer token).</param>
		/// <param name="headers">Custom headers.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The response content deserialized as the specified type.</returns>
		Task<TResponse> PatchAsJsonAsync<TRequest, TResponse>(string uri, TRequest objectContent, bool ignoreNull = true, AuthConfiguration authConfig = null, IDictionary<string, string> headers = null, CancellationToken? cancellationToken = null);

		#endregion
	}
}