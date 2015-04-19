using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GOALWorker
{
    public class GameListProcessor
    {
        private const string Url = @"http://live.nhle.com/GameData/RegularSeasonScoreboardv3.jsonp?loadScoreboard=?";

        private readonly List<Game> _gamesInProgress = new List<Game>();

        public Dictionary<string,string>[] GetGames()
        {
            WebRequest request = WebRequest.Create(new Uri(Url));
            Stream response = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response);
            string result = reader.ReadToEnd();
            Regex reg = new Regex(@"\[.*\]");
            result = reg.Match(result).ToString();
            Debug.Print(result);
            Dictionary<string, string>[] games = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(result);
            return games;
        }

        public void CheckToStartGameMonitor()
        {
            Trace.TraceInformation("GOALWorker is checking for games in progress that are not being monitored");
            var games = GetGames();
            var progressGames = games.Where(x => x["bsc"] == "progress");
            foreach (var item in progressGames)
            {
                if(_gamesInProgress.Any(x => x.Identifier == item["id"])) {continue;}
                var game = new Game { AwayTeam = item["atv"], HomeTeam = item["htv"], Identifier = item["id"] };
                _gamesInProgress.Add(game);
                Trace.TraceInformation("GOALWorker is starting a monitor for {0} vs {1}", game.HomeTeam, game.AwayTeam);
                var gameMonitor = new GameMonitor();
                Task t = Task.Run(() => gameMonitor.Start(game));
            }
            foreach (Game completedGame in _gamesInProgress.Where(x => x.Complete))
            {
                Trace.TraceInformation("GOALWorker is removing the Game for {0} vs {1}", completedGame.HomeTeam, completedGame.AwayTeam);
                _gamesInProgress.Remove(completedGame);
            }
        }
    }
}
