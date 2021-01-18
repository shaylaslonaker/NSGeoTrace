using System.Diagnostics;
using System.IO;

namespace SystemControl
{
    public class ConsoleAppExec
    {
        public ConsoleAppExec(string filename, string args)
        {
            _filename = filename;
            _arguments = args;
            this.Run();
        }
        private string _nsdata = null;
        public string Data
        {
            get
            {
                return _nsdata;
            }
        }
        private string _filename = null;
        public string FileName
        {
            get
            {
                return _filename;
            }
        }
        private string _arguments= @"-a -n -o";
        public string Aruguments
        {
            get
            {
                return _arguments;
            }
        }

        public void Run()
        {
            using (Process proc = new Process())
            {

                proc.StartInfo.FileName = _filename;
                proc.StartInfo.Arguments = _arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                _nsdata = proc.StandardOutput.ReadToEnd();

                //proc.WaitForExit();


            }
           

        }
    }
}
