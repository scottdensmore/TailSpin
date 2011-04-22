namespace TailSpin.PhoneClient.Services.Clients
{
    using System;
    using System.Globalization;
    using System.Net;

    public static class HttpClient
    {
        public static HttpWebRequest RequestTo(Uri surveysUri, string userName, string password)
        {
            var request = (HttpWebRequest)WebRequest.Create(surveysUri);
            var authHeader = string.Format(CultureInfo.InvariantCulture, "user {0}:{1}", userName, password);
            request.Headers[HttpRequestHeader.Authorization] = authHeader;
            return request;
        }
    }
}