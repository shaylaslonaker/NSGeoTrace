using System;

using System.Diagnostics;
using System.IO;
namespace NSGeoTrace
{
    public class NetstatController
    {
        private string _nsdata = null;
        public string Data
        {
            get
            {
                return _nsdata;
            }
        }

        public void Run()
        {
            try
            {

            }
            catch(Exception ex)
            {

                using (Process proc = new Process())
                {

                    proc.StartInfo.FileName = @"c:\windows\system32\netstat.exe";
                    proc.StartInfo.Arguments = @"-a -n -o";
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.Start();

                    _nsdata = proc.StandardOutput.ReadToEnd();

                    

                }
                if (File.Exists("@netstat.log"))
                {
                    File.Delete(@"netstat.log");
                }
                File.WriteAllText(@"netstat.log", _nsdata);

            }
        }
    }

}
