using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NSGeoTrace
{
    public class ApacheLogParser:GoogleEarthHandler
    {
        private string[] _fileobj;
        public string[] FileObj
        {
            get
            {
                return _fileobj;
            }
        }
        private List<string> _ipaddresses = new List<string>();
        public List<string> IpAddresses
        {
            get
            {
                return _ipaddresses;
            }
        }

        private object _logfile;
        public object LogFile
        {
            get
            {
                return _logfile;
            }
        }

        public ApacheLogParser(string _filepath)
        {
            _fileobj = System.IO.File.ReadAllLines(_filepath);

        }
        private void ParseData()
        {
            foreach(string s in _fileobj)
            {
                var string_split = s.Split('-');
                if (!_ipaddresses.Contains(string_split[0]))
                {
                    _ipaddresses.Add(string_split[0]);
                }
            }
        }
        public void Run()
        {
            ParseData();
        }
    }
}
