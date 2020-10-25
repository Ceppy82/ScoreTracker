

namespace ScoreTracker
{
    public static class Classes
    {
        public class Profile
        {
            public Playerinfo playerInfo { get; set; }
        }
        public class Playerinfo
        {
            public string playerId { get; set; }
            public string playerName { get; set; }
            public string avatar { get; set; }
            public bool tracked { get; set; }
            public bool compare { get; set; }
            public int rank { get; set; }
            public int countryRank { get; set; }
            public float pp { get; set; }
            public string country { get; set; }
            public string role { get; set; }
            public object[] badges { get; set; }
            public string history { get; set; }
            public int permissions { get; set; }
            public int inactive { get; set; }
            public int banned { get; set; }
            public string timestampu { get; set; }
            public string timestamp { get; set; }
        }

        public class Tracked
        {
            public Tracked(string timestampu, string playerId, string playerName, float pp, int rank, int countryRank, string country, string timestamp)
            {
                this.timestampu = timestampu;
                playerID = playerId;
                this.playerName = playerName;
                this.pp = pp;
                this.rank = rank;
                this.countryRank = countryRank;
                this.country = country;
                this.timestamp = timestamp;
            }

            public string timestampu { get; set; }
            public string playerID { get; set; }
            public string playerName { get; set; }
            public float pp { get; set; }
            public int rank { get; set; }
            public int countryRank { get; set; }
            public string country { get; set; }
            public string timestamp { get; set; }
        }






    }

}

