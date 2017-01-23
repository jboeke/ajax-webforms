using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using System.Web.Script.Services;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace WebFormsAjax
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(BufferResponse = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetCurrentTime()
        {
            string theMessage = "The Current Time is: " + DateTime.Now.ToString();

            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("success", "true");
            result.Add("msg", theMessage);

            string json = JsonConvert.SerializeObject(result, Formatting.None);

            return theMessage;
        }

        [WebMethod(BufferResponse = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRecentEarthquakes()
        {
            int daysBack = 1;
            int maxcount = 5;
            int count = 0;
            var startDate = DateTime.Now.AddDays(-1 * daysBack);
            var endDate = DateTime.Now;
            
            var startDateString = startDate.ToString("yyyy-MM-dd");
            var endDateString = endDate.ToString("yyyy-MM-dd");

            string url = $"http://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={startDateString}&endtime={endDateString}";

            string response = GetWebStringResponse(url);

            dynamic jo = JObject.Parse(response);
            
            // Wait 2 seconds for fun
            System.Threading.Thread.Sleep(2 * 1000);

            // Format the response the way we like.
            // We could also format the response in the jQuery AJAX response callback

            StringBuilder sb = new StringBuilder();

            foreach (var item in jo.features)
            {
                count++;
                sb.Append("<div>");
                sb.Append($"<p>Magnitude {item.properties.mag} at <a href='{item.properties.url}' target='_blank'>{item.properties.place}</a></p>");
                sb.Append("</div>");
                sb.Append("<hr />");
                if (count >= maxcount)
                {
                    break;
                }
            }

            return sb.ToString();
        }

        [WebMethod(BufferResponse = false)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetRandomImage(int size = 400)
        {
            string url = $"http://lorempixel.com/{size}/{size}/";

            // Getting the response and converting to base 64 is completely unnecessary since we could just embed the url itself.
            // But let's do it anyway...

            byte[] reponse = GetWebByteResponse(url);
            string base64String = Convert.ToBase64String(reponse);

            // Format the response the way we like.
            // We could also format the response in the jQuery AJAX response callback

            StringBuilder sb = new StringBuilder();
            
            sb.Append("<div>");
            sb.Append($"<img src='data:image/png;base64,{base64String}'/>");
            sb.Append("</div>");
            
            return sb.ToString();
        }

        private static string GetWebStringResponse(string url)
        {
            // Create a request for the URL.        
            WebRequest request = WebRequest.Create(url);

            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Display the status.
            //Console.WriteLine(response.StatusDescription);

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

        private static byte[] GetWebByteResponse(string url)
        {
            byte[] bArray = null;

            // Create a request for the URL.        
            WebRequest request = WebRequest.Create(url);

            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            // Display the status.
            //Console.WriteLine(response.StatusDescription);

            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(dataStream))
            {
                bArray = br.ReadBytes(500000);
                br.Close();
            }
            
            // Cleanup the streams and the response.
            dataStream.Close();
            response.Close();

            return bArray;
        }
    }

}