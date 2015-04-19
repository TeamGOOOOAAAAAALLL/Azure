using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;

namespace GOALWorker
{
    public class GameMonitor
    {
        private const string Url = @"http://live.nhle.com/GameData/RegularSeasonScoreboardv3.jsonp?loadScoreboard=?";

        public void Start(Game game)
        {
            game.Active = true;
            bool gameFinal = false;
            int homeTeamScore = 0;
            int awayTeamScore = 0;
            while (!gameFinal)
            {
                Trace.TraceInformation("GOALWorker is checking the score in Game for {0} vs {1}", game.HomeTeam, game.AwayTeam);
                string json = RetriveJson();
                var gamesData = ParseJson(json);
                Dictionary<string, string> gameData = gamesData.First(x => x["id"] == game.Identifier);
                int awayScoreCheck = 0;
                int homeScoreCheck = 0;
                int.TryParse(gameData["ats"], out awayScoreCheck);
                int.TryParse(gameData["hts"], out homeScoreCheck);
                if (homeTeamScore != homeScoreCheck)
                {
                    LightNotifier.NotifyLightsForTeam(game.HomeTeam);
                    homeTeamScore = homeScoreCheck;
                }
                if (awayTeamScore != awayScoreCheck)
                {
                    LightNotifier.NotifyLightsForTeam(game.AwayTeam);
                    awayTeamScore = awayScoreCheck;
                }
                gameFinal = gameData["bsc"] == "final";
                Thread.Sleep(1000);
            }
            game.Complete = true;
        }

        public string RetriveJson()
        {
            WebRequest request = WebRequest.Create(new Uri(Url));
            Stream response = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response);
            string result = reader.ReadToEnd();
            Regex reg = new Regex(@"\[.*\]");
            result = reg.Match(result).ToString();
            Debug.Print(result);
            return result;
        }


        public Dictionary<string, string>[] ParseJson(string json)
        {
            Dictionary<string, string>[] games = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(json);
            return games;
        }
    }
}
