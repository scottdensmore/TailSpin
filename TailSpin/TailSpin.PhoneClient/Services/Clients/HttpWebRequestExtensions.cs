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
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization.Json;
    using Microsoft.Phone.Reactive;

    public static class HttpWebRequestExtensions
    {
        public static IObservable<T> GetJson<T>(this HttpWebRequest request)
        {
            request.Method = "GET";
            request.Accept = "application/json";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                            {
                                using (var responseStream = response.GetResponseStream())
                                {
                                    var serializer = new DataContractJsonSerializer(typeof(T));
                                    return (T)serializer.ReadObject(responseStream);
                                }
                            });
        }

        public static IObservable<Unit> PostJson<T>(this HttpWebRequest request, T obj)
        {
            request.Method = "POST";
            request.ContentType = "application/json";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                            {
                                using (requestStream)
                                {
                                    var serializer = new DataContractJsonSerializer(typeof(T));
                                    serializer.WriteObject(requestStream, obj);
                                    requestStream.Close();
                                }

                                return
                                    Observable.FromAsyncPattern<WebResponse>(
                                        request.BeginGetResponse,
                                        request.EndGetResponse)();
                            },
                        (requestStream, webResponse) => new Unit());
        }
    }
}
