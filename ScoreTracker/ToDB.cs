using LiteDB;
using System;

namespace ScoreTracker
{
    class ToDB
    {
        public static void SaveProfiles()
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var _timestampu = DateTime.UtcNow.Ticks;
            var _timestamp = new DateTime(_timestampu).ToString();
            var col = _db.GetCollection<Classes.Playerinfo>("profiles");
            col.EnsureIndex(x => x.playerId);
            foreach (var item in ScoreTrackerController.profileList)
            {
                item.timestamp = _timestamp;
                item.timestampu = _timestampu.ToString();
                col.Upsert(item.playerId, item);
            }
            _db.Dispose();
        }

        public static void SaveTracked()
        {
            var _db = new LiteDatabase("Filename = " + ScoreTrackerController.ScoreTrackerDB + "; connection = Shared");
            _db.Pragma("UTC_DATE", true);
            var _timestampu = DateTime.UtcNow.Ticks;
            var _timestamp = new DateTime(_timestampu).ToString();
            foreach (var item in ScoreTrackerController.trackedList)
            {
                var col = _db.GetCollection<Classes.Tracked>("_" + item.playerID);
                col.EnsureIndex(x => x.timestampu);
                item.timestamp = _timestamp;
                item.timestampu = _timestampu.ToString();
                col.Upsert(item.timestampu, item);
            }
            _db.Dispose();
            SaveProfiles();
        }
    }
}
