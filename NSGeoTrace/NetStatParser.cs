using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace NSGeoTrace
{
    public class NetStatParser
    {

        private Dictionary<string, int> _ip_pid = new Dictionary<string, int>();
        public Dictionary<string,int> IPxPID
        {
            get
            {
                return _ip_pid;
            }
        }
        private string _filepath = @"netstat.log";


        public void Run()
        {
            var ret = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(_filepath);
                /*   for (int i = 4; i < lines.Count(); i++)
                   {

                       lines[i] = Regex.Replace(lines[i], @"\s+", " ");

                       string[] tmp = lines[i].Split(' ');
                       string[] ip_port = tmp[3].Split(':');



                       if (ip_port[0] != "127.0.0.1")
                       {

                           if (!_ip_pid.ContainsKey(ip_port[0]))
                           {

                               _ip_pid.Add(tmp[0], Convert.ToInt32(tmp[5]));
                           }
                           if (!ret.Contains(ip_port[0]))
                           {
                               ret.Add(ip_port[0]);

                           }

                       }


                   }*/

                Dictionary<int, string> dict_ip_pid = new Dictionary<int, string>();

                for (int i = 4; i < lines.Count(); i++)
                {

                    lines[i] = Regex.Replace(lines[i], @"\s+", " ");

                    string[] tmp = lines[i].Split(' ');
                    if(tmp.Length == 6)
                    {
                        string[] ip_port = tmp[3].Split(':');

                        string ip = ip_port[0];
                        string port = ip_port[1];
                        string pid = tmp[5];

                        if (ip != "127.0.0.1")
                        {

                            if (!_ip_pid.ContainsKey(ip))
                            {

                                _ip_pid.Add(ip, Convert.ToInt32(pid));
                            }


                        }
                    }
                   


                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }

        }


    }
}
