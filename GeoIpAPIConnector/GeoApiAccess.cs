namespace GeoIpAPI.DataAccess
{
    public class GeoApiAccess
    {
        private string _result = string.Empty;
        public string Result
        {
            get
            {
                return _result;
            }
        }
        private WebRequestHandler htReqHandler = new WebRequestHandler();
        public void GeoApiRequest(string ipAddress,string access_key)
        {
            
            string queryStr = string.Concat(@"http://api.ipapi.com/",ipAddress,"?access_key=", access_key);
            HtmlAgilityPack.HtmlDocument x = htReqHandler.HtDocument(queryStr);
            if(x != null)
            {
                _result = x.Text;
            }

        }
        
    }
 
}
