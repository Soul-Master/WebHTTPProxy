using System;
using System.IO;
using System.Net;
using System.Web;

namespace WebProxy
{
    public class ProxyHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var req = context.Request;
            var domain = req.QueryString["domain"];
            var path = req.Url.PathAndQuery;

            if (string.IsNullOrEmpty(domain)) throw new Exception("`domain` query string is required.");
            if (string.IsNullOrEmpty(path)) throw new Exception("Url path is required.");

            var webRequest = (HttpWebRequest)WebRequest.Create(domain + path);
            webRequest.Method = context.Request.HttpMethod;
            webRequest.Credentials = CredentialCache.DefaultNetworkCredentials;
            using (var response = (HttpWebResponse)webRequest.GetResponse())
            {
                context.Response.StatusCode = (int)response.StatusCode;
                context.Response.Status = string.Format("{0} {1}", (int)response.StatusCode, response.StatusCode);

                var contentType = response.ContentType;
                if (contentType == "application/json" && !string.IsNullOrEmpty(req.QueryString["callback"]))
                {
                    // Replace content type of all JSONP requests to be plain javascript
                    contentType = "text/javascript";
                }

                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.ContentType = contentType;
                context.Response.Write(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }
        }

        #endregion
    }
}
