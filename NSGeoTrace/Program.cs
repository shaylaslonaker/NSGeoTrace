// Decompiled with JetBrains decompiler
// Type: NSGeoTrace.Program
// Assembly: NSGeoTrace, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 138666A4-5550-4537-8014-40438833AB04
// Assembly location: D:\Projects\netstat2google-earth-master\NSGeoTrace\bin\Debug\NSGeoTrace.exe
using System.Text;
using GeoIpAPI.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SystemControl;


namespace NSGeoTrace
{

    delegate void pooptaco();
    internal static class Program
    {
        private static NetstatController nsctrl = new NetstatController();
        private static NetStatParser nsparse = new NetStatParser();
        private static GoogleEarthHandler geh = new GoogleEarthHandler();
        private static GeoApiAccess api = new GeoApiAccess();
        private static DataTable _dt_report = new DataTable();
        private static Dictionary<string, object> dict_ip_2_geoloc = new Dictionary<string, object>();
        private static Dictionary<int, string> dict_process_id_x_name = new Dictionary<int, string>();

        private static string apiKey = ConfigurationManager.AppSettings["geoapikey"].ToString();
        private static string _nsData;
        private static string _tlData;

        static string nsData
        {
            get
            {
                return _nsData;
            }
        }
        static string tlData
        {
            get
            {
                return _tlData;
            }
        }

        private static void collector_netstat()
        {
            string nsData = string.Empty;
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {

                proc.StartInfo.FileName = @"c:\windows\system32\netstat.exe";
                proc.StartInfo.Arguments = @"-a -n -o";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                nsData = proc.StandardOutput.ReadToEnd();



            }
        }

        private static void collector_tasklist()
        {
            string tlData = string.Empty;

            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {

                proc.StartInfo.FileName = @"c:\windows\system32\tasklist.exe";
                proc.StartInfo.Arguments = @"";
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                tlData = proc.StandardOutput.ReadToEnd();


            }
            _tlData = tlData;
        }
        private static void systemDataCollector()
        {

            var cApp = new ConsoleAppExec(@"c:\windows\system32\tasklist", string.Empty);
            // string cmd1 = @"c:\windows\system32\tasklist.exe > tasklist.log & c:\windows\system32\netstat.exe -a -n -o >netstat.log";
            // File.WriteAllText(@"tmp.cmd", cmd1);
            collector_netstat();
            collector_tasklist();




            char[] delims = new[] { '\r', '\n' };
            string[] tlDataArr = tlData.Split(delims);
            List<string> tlDataArrTmp = new List<string>();
            for (int i = 5; i < tlDataArr.Count(); i++)
            {
                if (tlDataArr[i].ToString().Length > 0)
                {

                    tlDataArrTmp.Add(tlDataArr[i].ToString());

                }
            }
            tlDataArr = tlDataArrTmp.ToArray();

            Console.WriteLine("done");
            #region deprecated code
            /*
string[] lines = cApp.Data.Split(new[]
       {
            Environment.NewLine
        }, StringSplitOptions.None);
int i = 0;
  for (int n = 4; n < lines.Count(); n++)
{
    string cleaned = Regex.Replace(lines[n], @"\s+", " ");
    string[] s_split = cleaned.Split(' ');
    if(s_split[1] == "Idle")
    {
        s_split[1] = (-1).ToString();
    }
    int pid = -1;


    for (int x = 0; x < 3; x++)
    {
        bool isNumeric = int.TryParse(s_split[x], out n);
        if (isNumeric)
        {
            pid = n;
            break;
        }
    }


    if (pid > -1)
    {
        try
        {
            dict_process_id_x_name.Add( Convert.ToInt32(Convert.ToInt32(s_split[1])), s_split[0]);
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }


    }

}

*/
            #endregion
        }
        private static void Main(string[] args)
        {
            var geh = new GoogleEarthHandler();
            var logparser = new ApacheLogParser(@"access.log");
            logparser.Run();
            _dt_report.Columns.Add("ip_addr", typeof(string));
            _dt_report.Columns.Add("pid", typeof(int));
            _dt_report.Columns.Add("latitude", typeof(string));
            _dt_report.Columns.Add("longitude", typeof(string));
            _dt_report.Columns.Add("city", typeof(string));
            _dt_report.Columns.Add("state", typeof(string));
            _dt_report.Columns.Add("process_name", typeof(string));
            _dt_report.Columns.Add("zip", typeof(string));
            foreach (string s in logparser.IpAddresses)
            {
                DataRow dr = _dt_report.NewRow();
                dr["ip_addr"] = s;
                api.GeoApiRequest(dr["ip_addr"].ToString(), apiKey);


                dynamic apiResult = JsonConvert.DeserializeObject(api.Result);


                dr["latitude"] = apiResult.latitude;
                dr["longitude"] = apiResult.longitude;
                dr["city"] = apiResult.city;
                dr["state"] = apiResult.region_name;
                dr["zip"] = apiResult.zip;
                _dt_report.Rows.Add(dr);
                Console.WriteLine("IP: " + dr["ip_addr"] + " | Location City: " + dr["city"] + " Location State: " + dr["state"]);
                dr = null;
            }

            geh.Run(_dt_report);

            if (File.Exists(@".\doc.kml"))
            {
                string pwd = Directory.GetCurrentDirectory();
                new ConsoleAppExec(ConfigurationManager.AppSettings["google_earth_exe"].ToString(), string.Concat(pwd, @"\doc.kml"));
            }
        }
            /*
            private static void Main(string[] args)
            {

                systemDataCollector();

                _dt_report.Columns.Add("ip_addr", typeof(string));
                _dt_report.Columns.Add("pid", typeof(int));
                _dt_report.Columns.Add("latitude", typeof(string));
                _dt_report.Columns.Add("longitude", typeof(string));
                _dt_report.Columns.Add("city", typeof(string));
                _dt_report.Columns.Add("state", typeof(string));
                _dt_report.Columns.Add("process_name", typeof(string));
                _dt_report.Columns.Add("zip", typeof(string));


                //netstat execute 
                nsctrl.Run();
                //netstat parser
                nsparse.Run();


                foreach (KeyValuePair<string, int> k in nsparse.IPxPID)
                {
                    if (k.Key != "0.0.0.0")
                    {
                        DataRow dr = _dt_report.NewRow();
                        dr["ip_addr"] = k.Key;
                        dr["pid"] = k.Value;
                        api.GeoApiRequest(dr["ip_addr"].ToString(), apiKey);


                        dynamic apiResult = JsonConvert.DeserializeObject(api.Result);


                        dr["latitude"] = apiResult.latitude;
                        dr["longitude"] = apiResult.longitude;
                        dr["city"] = apiResult.city;
                        dr["state"] = apiResult.region_name;
                        dr["zip"] = apiResult.zip;
                        var cApp = new ConsoleAppExec(@"c:\windows\system32\tasklist", string.Empty);
                        string[] lines = cApp.Data.Split(new[]
                        {
                            Environment.NewLine
                        }, StringSplitOptions.None);

                        int stepskip = 7;
                        int iteration = 0;
                        foreach (string s in lines)
                        {

                            if(iteration >= stepskip)
                            {
                                string cleaned = Regex.Replace(s, @"\s+", " ");
                                string[] s_split = cleaned.Split(' ');
                                if (s_split.Count() >= 4)
                                {


                                    try
                                    {
                                        string procName = s_split[0];
                                        int pid = Convert.ToInt32(s_split[1]);
                                        int netstat_procNum = Convert.ToInt32(dr[1]);
                                        if (pid == netstat_procNum)
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            sb.Append("Tasklist PID: ");
                                            sb.Append(pid.ToString());


                                            sb.Append(" vs Netstat PID: ");
                                            sb.Append(dr["pid"]);
                                            Console.WriteLine(sb.ToString());
                                            sb.Clear();


                                            dr["process_name"] = procName;
                                            _dt_report.Rows.Add(dr);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (ex.InnerException != null)
                                        {
                                        }
                                    }
                                }

                            }
                            //_dt_report.Rows.Add(dr);
                            iteration++;
                        }

                    }

                }

                geh.Run(_dt_report);
                if (File.Exists(@".\doc.kml"))
                {
                    string pwd = Directory.GetCurrentDirectory();
                    new ConsoleAppExec(ConfigurationManager.AppSettings["google_earth_exe"].ToString(), string.Concat(pwd, @"\doc.kml"));
                }


            }
            */
        }
    }
