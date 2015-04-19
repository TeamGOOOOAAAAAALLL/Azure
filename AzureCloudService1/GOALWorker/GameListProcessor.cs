using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;

namespace GOALWorker
{
    public class GameListProcessor
    {
        public List<Games> GameList { get; set; }

        public void GetGames()
        {
            DateTime currentDate = DateTime.Now;
            string URL = string.Format("http://live.nhle.com/GameData/GCScoreboard/{0}.jsonp", currentDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            WebRequest request = WebRequest.Create(new Uri(URL));
            var response = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response);
            var result = reader.ReadToEnd();
            result = result.Substring(15, result.Length - 17);
            Debug.Print(result);
        }

        public void ProcessGameList()
        {

        }
    }
}
