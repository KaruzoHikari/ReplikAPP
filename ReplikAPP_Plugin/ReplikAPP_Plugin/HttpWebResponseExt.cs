using System.Net;

namespace ReplikAPP_Plugin
{
    
        public static class HttpWebResponseExt
        {
            public static HttpWebResponse GetResponseNoException(this HttpWebRequest req)
            {
                try
                {
                    return (HttpWebResponse)req.GetResponse();
                }
                catch (WebException we)
                {
                    var resp = we.Response as HttpWebResponse;
                    if (resp == null)
                        throw;
                    return resp;
                }
            }
        }
    
}