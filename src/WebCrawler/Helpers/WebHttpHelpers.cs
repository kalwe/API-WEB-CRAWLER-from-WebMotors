using System.Net;

//  Metodos para fazer check list no http response
public class WebHttpHelpers
{
    public long ContentLength { get; set; }
    public bool ContentExist { get; internal set; }
    public bool StatusOK { get; internal set; }

    // Return true only if http request return status code 200 
    public bool HttpResponseStatusOK(HttpWebResponse httpResponse)
    {
        if (httpResponse.StatusCode == HttpStatusCode.OK)
        {
            StatusOK = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Return true if HttpResponseStatusOK() return true
    //and ContentLength > 0
    public bool HttpResponseContentExist(HttpWebResponse httpResponse)
    {
        if (HttpResponseStatusOK(httpResponse) && httpResponse.ContentLength > 0)
        {
            ContentLength = httpResponse.ContentLength;
            ContentExist = true;
            return true;
        }
        else
        {
            return false;
        }
        
    }
}

