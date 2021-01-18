using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace NSGeoTrace
{
    public class GoogleEarthHandler
    {
        /* (out of place text to replace: [[replaceme]]
         * 
         * (template)
         
         <Placemark>
        <name>Simple placemark</name>
        <description>Attached to the ground. Intelligently places itself at the
          height of the underlying terrain.</description>
        <Point>
          <coordinates>-122.0822035425683,37.42228990140251,0</coordinates>
        </Point>
      </Placemark>
      
        
    */

        private string _kmldocument;
        public string KmlDocument
        {
            get
            {
                return _kmldocument;
            }
        }
        private string _kmlDoc = File.ReadAllText(@".\resources\template.kml");
        private string _kmlPlacemarkXml = @"<Placemark>
        <name>--name--</name>
<styleUrl>#msn_cross-hairs</styleUrl>        
<description>--desc--</description>
        <Point>
          <coordinates>--lon--,--lat--,0</coordinates>
        </Point>
      </Placemark>";

        public void Run(DataTable dtReport)
        {
            StringBuilder sb = new StringBuilder();
            foreach(DataRow dr in dtReport.Rows)
            {
                string tmp = _kmlPlacemarkXml;
                string humanReadableLoc = string.Concat("City: ", dr["city"], ", State:  ",dr["state"]);
                string processinfo = string.Concat("pid: ", dr["pid"], " image name:", dr["process_name"]);
                tmp = tmp.Replace("--name--", dr["ip_addr"].ToString());
                tmp = tmp.Replace("--desc--", string.Concat(humanReadableLoc,Environment.NewLine,processinfo));
                tmp = tmp.Replace("--lat--", dr["latitude"].ToString());
                tmp = tmp.Replace("--lon--", dr["longitude"].ToString());
                tmp = string.Concat(tmp, Environment.NewLine);
                sb.Append(tmp);

            }
            string documentData = _kmlDoc.Replace("--replaceme--", sb.ToString());
            if (File.Exists(@"doc.kml"))
            {
                File.Delete(@"doc.kml");
            }

            File.WriteAllText(@"doc.kml", documentData);
        }
    }
}
