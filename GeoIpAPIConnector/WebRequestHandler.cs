using HtmlAgilityPack;
using System;
using System.Net.Http;

namespace GeoIpAPI.DataAccess
{
    public class WebRequestHandler
    {
        public HtmlDocument HtDocument(string url)
        {
            HtmlDocument htDoc = null;
            try
            {
                HttpClient httpCli = new HttpClient();
                httpCli.DefaultRequestHeaders.Add("User-Agent", @"Mozilla/5.0 (Windows NT 10.0; WOW64; rv:61.0) Gecko/20100101 Firefox/61.0");
                httpCli.Timeout = new TimeSpan(0, 0, 5);
                var html = new HttpClient().GetStringAsync(url);
                htDoc = new HtmlDocument();
                htDoc.LoadHtml(html.Result);
                html.Dispose();

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
                return htDoc;

        }
    }
}
