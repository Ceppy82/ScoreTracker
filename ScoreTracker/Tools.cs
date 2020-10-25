using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScoreTracker
{
    class Tools
    {

        public static List<object> GetAllNames()
        {
            List<object> allNames = new List<object>();
            foreach (var item in ScoreTrackerController.profileList) allNames.Add(item.playerName);
            return allNames;
        }



        public static List<string> GetAllIDs(string _newIDs)
        {
            List<string> _IDlist = new List<string>();

            foreach (var item in ScoreTrackerController.profileList)
            {
                _IDlist.Add(item.playerId);
            }
            List<string> _newIDlist = new List<string>();


            if (_newIDs.Count(char.IsDigit) >= 17 && _newIDs.Length >= 17)
            {
                // Split on one or more non-digit characters.
                string[] numbers = Regex.Split(_newIDs, @"\D+");
                foreach (string value in numbers)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        long i = long.Parse(value);
                        if (i.ToString().Length == 17)
                        {
                            if (!_IDlist.Contains(i.ToString()))
                            {
                                Console.WriteLine(i.ToString() + " is now in the system. Check Tracked/Compared!");
                                _newIDlist.Add(i.ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("no ID given or not valid");
            }
            return _newIDlist;
        }

        public static async void Delayed(string _method, string _args)
        {
            ScoreTrackerController.delayCounter++;
            int _delay;
            _delay = 2000; //milliseconds
            await Task.Delay(ScoreTrackerController.delayCounter * _delay); // wait for n milliseconds
            if (_method == "GetScoreSaberData")
            {
                GetScoreSaberData(_args.ToString());
            }

            if (_method == "UpdateScoreSaberData")
            {
                UpdateScoreSaberData(_args);
            }

            ScoreTrackerController.delayCounter--;
        }

        public static void GetScoreSaberData(string _ScoreSaberID)
        {
            string _url = "https://new.scoresaber.com/api/player/" + _ScoreSaberID + "/basic";
            string _json = new WebClient().DownloadString(_url);
            var _newProfile = JsonConvert.DeserializeObject<Classes.Profile>(_json);
            var _newProfile2 = _newProfile.playerInfo;
            _newProfile2.compare = true;
            _newProfile2.tracked = true;
            ScoreTrackerController.profileList.Add(_newProfile2);
            ScoreTrackerController.trackedList.Add(new Classes.Tracked(_newProfile2.timestampu,
                _newProfile2.playerId, _newProfile2.playerName, _newProfile2.pp,
                _newProfile2.rank, _newProfile2.countryRank,
                _newProfile2.country, _newProfile2.timestamp));
            ToDB.SaveProfiles();
            ToDB.SaveTracked();
        }


        public static void UpdateScoreSaberData(string _ScoreSaberID)
        {
            string _url = "https://new.scoresaber.com/api/player/" + _ScoreSaberID + "/basic";
            string _json = new WebClient().DownloadString(_url);
            var _newProfile = JsonConvert.DeserializeObject<Classes.Profile>(_json);
            var _newProfile2 = _newProfile.playerInfo;

            for (int i = 0; i < ScoreTrackerController.profileList.Count(); i++)
            {
                bool _save = false;
                if (ScoreTrackerController.profileList[i].playerId == _newProfile2.playerId)
                {

                    foreach (var item in ScoreTrackerController.trackedList)
                    {
                        if (item.playerID == _newProfile2.playerId)
                        {
                            item.pp = _newProfile2.pp;
                            item.rank = _newProfile2.rank;
                            item.countryRank = _newProfile2.countryRank;
                            if (ScoreTrackerController.profileList[i].rank != _newProfile2.rank)
                            {
                                _save = true;
                            }
                            else if (ScoreTrackerController.profileList[i].countryRank != _newProfile2.countryRank)
                            {
                                _save = true;
                            }
                            else if (ScoreTrackerController.profileList[i].pp != _newProfile2.pp)
                            {
                                _save = true;
                            }
                        }
                    }
                    ScoreTrackerController.profileList[i].avatar = _newProfile2.avatar;
                    ScoreTrackerController.profileList[i].badges = _newProfile2.badges;
                    ScoreTrackerController.profileList[i].banned = _newProfile2.banned;
                    ScoreTrackerController.profileList[i].country = _newProfile2.country;
                    ScoreTrackerController.profileList[i].countryRank = _newProfile2.countryRank;
                    ScoreTrackerController.profileList[i].history = _newProfile2.history;
                    ScoreTrackerController.profileList[i].inactive = _newProfile2.inactive;
                    ScoreTrackerController.profileList[i].permissions = _newProfile2.permissions;
                    ScoreTrackerController.profileList[i].playerName = _newProfile2.playerName;
                    ScoreTrackerController.profileList[i].pp = _newProfile2.pp;
                    ScoreTrackerController.profileList[i].rank = _newProfile2.rank;
                    ScoreTrackerController.profileList[i].role = _newProfile2.role;


                    if (_save)
                    {
                        ToDB.SaveTracked();
                    }

                    break;
                }
            }

        }

        public static void UpdateProfiles(string _ID)
        {
            Delayed("UpdateScoreSaberData", _ID);
        }
    }
}
