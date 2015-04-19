
using System;

namespace GOALWorker
{
    public class Game
    {
        public string Identifier { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public bool Active { get; set; }
        public bool Complete { get; set; }
    }
}
