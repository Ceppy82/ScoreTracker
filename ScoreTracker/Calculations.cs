using System;
using System.Collections.Generic;
using System.Linq;

namespace ScoreTracker
{
    class Calculations
    {
        public static void GetProgress()
        {
            foreach (var item in ScoreTrackerController.trackedList)
            {
                var (MinG, MaxG, MinR, MaxR, ProgressG, ProgressR) = FromDB.GetMinMax(item.playerID);
                Console.WriteLine(item.playerName + "\'s progress, since the beginning of the recordings amounts to:\n" +
                    "global " + ProgressG.ToString() + " (best " + MinG.ToString() + ") (worst " + MaxG.ToString() + ")\n" +
                    "regional " + ProgressR.ToString() + " (best " + MinR.ToString() + ") (worst " + MaxG.ToString() + ")\n\n");
            }
        }
        public static void Compare()
        {
            var _compare = FromDB.GetComparedFromDB();
            Console.WriteLine("begin compare");
            List<(string _id, string _name, int _rank)> _compareList = new List<(string, string, int)>();
            List<string> _text = new List<string>();
            _text.Add("");
            foreach (var item in ScoreTrackerController.profileList)
            {
                foreach (var item2 in _compare)
                {
                    if (item.playerId == item2)
                    {
                        var _temp = ((item.playerId, item.playerName, item.rank));
                        _compareList.Add(_temp);
                    }
                }
            }
            if (_compareList.Count >= 2)
            {
                for (int i = 0; i < _compareList.Count(); i++)
                {
                    _text.Add(_compareList[i]._name + " (" + _compareList[i]._rank + ")");
                    foreach (var item in _compareList)
                    {
                        if (_compareList[i]._id != item._id)
                        {
                            int _distance = _compareList[i]._rank - item._rank;
                            if (_distance < 0)
                            {
                                _distance = _distance * -1;
                                _text.Add(_distance.ToString() + " ranks ahead of " + item._name);
                            }
                            else
                            {
                                _text.Add(_distance.ToString() + " ranks behind of " + item._name);
                            }
                        }
                    }
                    _text.Add("");
                }
            }
            foreach (var item in _text)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("end compare");
        }
    }
}
