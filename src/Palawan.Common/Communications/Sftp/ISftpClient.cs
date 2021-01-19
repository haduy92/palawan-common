using System;
using System.Collections.Generic;
using System.IO;

namespace Palawan.Common.Communications.Sftp
{
	public interface ISftpClient : IDisposable
	{
		bool IsConnected();
		void Connect();
		void Disconnect();
		bool Exists(string filePath);
		void RenameFile(string oldPath, string newPath);
		void DeleteFile(string filePath);
		void UploadFile(Stream input, string path);
		IEnumerable<FileInfo> GetFilesInFolder(string folderPath);
		MemoryStream ReadAllBytes(string fileName);
	}
}