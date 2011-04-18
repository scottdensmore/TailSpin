//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


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