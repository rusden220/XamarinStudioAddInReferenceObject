using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace XamarinStudioAddIn
{
	public class DebugProvider
	{
		public DebugProvider ()
		{
		}
		public static void Write(string str)
		{
			using (var stw = new StreamWriter ("XamarinStudioAddIn_Debug.txt", true, Encoding.UTF8)) {
				stw.WriteLine (DateTime.Now.ToString() + " " + str);
			}
		}
	}
}

