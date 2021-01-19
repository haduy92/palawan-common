using System;

namespace Palawan.Common.Communications.Sftp
{
	public class FileInfo
	{
		public string Name { get; set; }
		public string FullName { get; set; }
		public DateTime LastWriteTimeUtc { get; set; }
	}
}