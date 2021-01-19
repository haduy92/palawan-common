using System.IO;

namespace Palawan.Common.Communications.Sftp
{
	public class SftpPrivateKey
	{
		public Stream FileStream { get; set; }
		public string PassPhrase { get; set; }
	}
}