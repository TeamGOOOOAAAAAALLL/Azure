using System.Diagnostics;
using System.Net;
using System.Text;
using System.IO;

namespace GOALWorker
{
    public static class LightNotifier
    {
        //TODO: Jared Implement call to each light here for their team
        public static void NotifyLightsForTeam(string teamname)
        {
            Trace.TraceInformation("GOALWorker is Sending Light Request for {0}", teamname);

            WebRequest request = WebRequest.Create("https://api.spark.io/v1/devices/DEVICEID/score");

            request.Method = "POST";

            string postData = "access_token=ACCESSTOKEN&params=0%2C255%2C0";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
           request.GetResponse();
            
        }
    }
}
