# WebHTTPProxy
Simplest ASP.NET application to proxy any income request for IIS 7+ (integrated pipeline mode)

To make request to this url

https://bx.in.th/api/chart/price/?pairing=1&int=720&limit=360

You can simply request to this url instead

http://`yourdomain.com`/api/chart/price/?pairing=1&int=720&limit=360&domain=`https://bx.in.th`

This service should work for any HTTP methods because service create web request based on your HTTP request.

```cs
var webRequest = (HttpWebRequest)WebRequest.Create(domain + path);
webRequest.Method = context.Request.HttpMethod;
```
