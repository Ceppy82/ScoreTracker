using LiteDB;
using System.Collections.Generic;

namespace ScoreTracker
{
    class FromDB
    {

        public static List<Classes.Playerinfo> GetProfilesAsList()
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var col = _db.GetCollection<Classes.Playerinfo>("profiles");
            col.EnsureIndex(x => x.playerId);
            var results = col.FindAll();
            List<Classes.Playerinfo> _profileList = new List<Classes.Playerinfo>();
            foreach (var item in results)
            {
                _profileList.Add(item);
            }
            _db.Dispose();
            return _profileList;
        }

        public static List<string> GetTrackedFromDB()
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var col = _db.GetCollection<Classes.Playerinfo>("profiles");
            col.EnsureIndex(x => x.playerId);
            var results = col.Find(x => x.tracked);
            List<string> _tracked = new List<string>();
            foreach (var item in results)
            {
                _tracked.Add(item.playerId);
            }

            return _tracked;
        }

        public static List<string> GetComparedFromDB()
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var col = _db.GetCollection<Classes.Playerinfo>("profiles");
            col.EnsureIndex(x => x.playerId);
            var results = col.Find(x => x.compare);
            List<string> _compared = new List<string>();
            foreach (var item in results)
            {
                _compared.Add(item.playerId);
            }
            return _compared;
        }

        public static (int MinG, int MaxG, int MinR, int MaxR, int ProgressG, int ProgressR) GetMinMax(string _ID)
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var col = _db.GetCollection<Classes.Playerinfo>("_" + _ID);
            col.EnsureIndex(x => x.timestampu);
            var _resultMinG = col.Min(x => x.rank);
            var _resultMinR = col.Min(x => x.countryRank);
            var _resultMaxG = col.Max(x => x.rank);
            var _resultMaxR = col.Max(x => x.countryRank);
            return (_resultMinG, _resultMaxG, _resultMinR, _resultMaxR,
                _resultMinG - _resultMaxG, _resultMinR - _resultMaxR);
        }
    }
}
