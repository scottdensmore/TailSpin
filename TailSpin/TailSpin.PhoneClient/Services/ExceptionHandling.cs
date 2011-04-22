namespace TailSpin.PhoneClient.Services
{
    using System.Net;

    public static class ExceptionHandling
    {
        public static TaskCompletedSummary GetSummaryFromWebException(string taskName, WebException e)
        {
            var webResponse = e.Response as HttpWebResponse;
            if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                //// "Access denied // check credentials"
                return new TaskCompletedSummary { Task = taskName, Result = TaskSummaryResult.AccessDenied };
            }

            string response;
            using (var stream = e.Response.GetResponseStream())
            {
                var buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                response = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }

            if (string.IsNullOrEmpty(response))
            {
                //// "Can not connect to server // check conectivity";
                return new TaskCompletedSummary { Task = taskName, Result = TaskSummaryResult.UnreachableServer };
            }

            return new TaskCompletedSummary { Task = taskName, Result = TaskSummaryResult.UnknownError };
        }
    }
}