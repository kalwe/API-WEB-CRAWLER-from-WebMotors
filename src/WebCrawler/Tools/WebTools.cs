using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text;

namespace WebCrawler.Tools
{
    public class WebTools : WebHttpHelpers
    {
        public HttpWebResponse WebResponse { get; internal set; }

        // Set variable HttResponse with response asynchronously from url
        public async Task GetUrlResponse(Uri url)
        {
            // Initialize new instance for the url
            // and return response asynchronously
            var HttpRequest = WebRequest.CreateHttp(url);
            WebResponse = (HttpWebResponse)await HttpRequest.GetResponseAsync();
        }

        // Return a StreamReader with webresponse from web request
        public StreamReader GetHttpResponseStream(HttpWebResponse httpResponse)
        {
            StreamReader Stream = null;
            // if (httpResponse.StatusCode == HttpStatusCode.OK)
            //     Stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);
            if (HttpResponseStatusOK(httpResponse))
                Stream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8);

            return Stream;
        }
    }

}