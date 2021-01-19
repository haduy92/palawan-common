using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using Serilog;
using SSH = Renci.SshNet;

namespace Palawan.Common.Communications.Sftp
{
	public class SftpClient : ISftpClient
	{
		private readonly SSH.SftpClient _sftpClient;
		private readonly ILogger _logger;

		public SftpClient(string host, int port, string username, string password)
		{
			_sftpClient = new SSH.SftpClient(host, port, username, password);
		}

		public SftpClient(string host, int port, string username, string password, ILogger logger) : this(host, port, username, password)
		{
			_logger = logger;
			_logger.Information("Creating {Type} using User/Pass authentication with Host:{Host}, Port:{Port}."
				, typeof(SftpClient), host, port);
		}

		public SftpClient(IOptions<SftpServerOptions> sftpOptions, ILogger logger)
			: this(sftpOptions.Value.Host, sftpOptions.Value.Port, sftpOptions.Value.UserName, sftpOptions.Value.Password, logger)
		{ }

		public SftpClient(string host, int port, string username, params SftpPrivateKey[] sftpKeys)
		{
			if (sftpKeys == null || sftpKeys.Length == 0)
			{
				throw new ArgumentException("SFTP Keys are required.", nameof(sftpKeys));
			}

			var sshPrivateKeys = sftpKeys
				.Select(x => string.IsNullOrWhiteSpace(x.PassPhrase)
					? new SSH.PrivateKeyFile(x.FileStream)
					: new SSH.PrivateKeyFile(x.FileStream, x.PassPhrase))
				.ToArray();
			_sftpClient = new SSH.SftpClient(host, port, username, sshPrivateKeys);
		}

		public SftpClient(string host, int port, string username, ILogger logger, params SftpPrivateKey[] sftpKeys)
			: this(host, port, username, sftpKeys)
		{
			_logger = logger;
			_logger.Information("Creating {Type} using {KeyType} authentication with Host:{Host} Port:{Port}."
				, typeof(SftpClient), typeof(SftpPrivateKey), host, port);
		}

		public void Connect()
		{
			_sftpClient.Connect();
		}

		public bool IsConnected()
		{
			return _sftpClient.IsConnected;
		}

		public void Disconnect()
		{
			_sftpClient.Disconnect();
		}

		public bool Exists(string filePath)
		{
			_logger.Information("Checking if file exists at path {FilePath}.", filePath);
			return _sftpClient.Exists(filePath);
		}

		public void RenameFile(string oldPath, string newPath)
		{
			_logger.Information("Renaming file from {OldPath} to {NewPath}.", oldPath, newPath);
			_sftpClient.RenameFile(oldPath, newPath);
		}

		public void DeleteFile(string filePath)
		{
			_logger.Information("Deleting file at path {FilePath}.", filePath);
			_sftpClient.DeleteFile(filePath);
		}

		public void UploadFile(Stream input, string path)
		{
			_logger.Information("Uploading file stream to path {FilePath}.", path);
			_sftpClient.UploadFile(input, path);
		}

		public IEnumerable<FileInfo> GetFilesInFolder(string folderPath)
		{
			_logger.Information("Getting files in folder {FolderPath}.", folderPath);

			return _sftpClient.ListDirectory(folderPath)?.Select(file => new FileInfo()
			{
				Name = file.Name,
				FullName = file.FullName,
				LastWriteTimeUtc = file.LastWriteTimeUtc,
			});
		}

		public MemoryStream ReadAllBytes(string fileName)
		{
			_logger.Information("Reading file at path {FilePath} as stream.", fileName);
			return new MemoryStream(_sftpClient.ReadAllBytes(fileName));
		}

		public void Dispose()
		{
			_sftpClient.Disconnect();
			_sftpClient.Dispose();
		}
	}
}