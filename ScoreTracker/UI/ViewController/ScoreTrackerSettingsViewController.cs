using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage;
using HMUI;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Drawing;
using Color = UnityEngine.Color;
using BeatSaverSharp;
using UnityEngine.UI;
using BeatSaberMarkupLanguage.Tags;
using BeatSaberMarkupLanguage.Parser;
using BS_Utils.Utilities;
using ScoreTracker;

namespace ScoreTracker.UI.ViewController
{
    public class ScoreTrackerSettingsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "ScoreTracker.UI.BSML.settings.bsml";


        [UIParams]
        BSMLParserParams parserParams;



        [UIValue("enabled")]
        private bool enabled = ScoreTrackerController.ConfigVariable.GetBool("settings", "enabled");


        [UIValue("saved-profiles")]
        private List<object> savedprofiles = Tools.GetAllNames();


        [UIValue("tracking-enabled")]
        private bool trackingenabled
        {
            get 
            {
                Plugin.Log?.Debug($"{selectedprofile} get trackingenabled");

                foreach (var item in ScoreTrackerController.profileList)
                {
                    if (item.playerName == selectedprofile)
                    {
                        Plugin.Log?.Debug($"{selectedprofile} {item.tracked.ToString()} get trackingenabled");
                        return item.tracked;
                    }
                }
                return true;
            }
            set
            {
                Plugin.Log?.Debug($"{selectedprofile} set trackingenabled");

                for (int i = 0; i < ScoreTrackerController.profileList.Count(); i++)
                {
                    if (ScoreTrackerController.profileList[i].playerName == selectedprofile)
                    {
                        ScoreTrackerController.profileList[i].tracked = trackingenabled;
                        ToDB.SaveProfiles();
                        break;
                    }
                }
            }
        }
            

        [UIValue("comparing-enabled")]
        private bool comparingenabled

        {
            get
            {
                Plugin.Log?.Debug($"{selectedprofile} get comparingenabled");
                foreach (var item in ScoreTrackerController.profileList)
                {
                    if (item.playerName == selectedprofile)
                    {
                        Plugin.Log?.Debug($"{selectedprofile} {item.compare} get comparingenabled");
                        return item.compare;
                    }
                }
                Plugin.Log?.Debug($"{selectedprofile} get comparingenabled ERROR");

                return true;
            }
            set
            {
                Plugin.Log?.Debug($"{selectedprofile} set comparingenabled");

                for (int i = 0; i < ScoreTrackerController.profileList.Count(); i++)
                {
                    if (ScoreTrackerController.profileList[i].playerName == selectedprofile)
                    {
                        ScoreTrackerController.profileList[i].compare = comparingenabled;
                        ToDB.SaveProfiles();
                        break;
                    }
                }
            }
        }


        [UIValue("selected-profile")]
        private string selectedprofile = ScoreTrackerController.profileList[0].playerName;

        
        [UIAction("profile-changed")]
        protected void ProfileChanged()
        {
            foreach (var item in ScoreTrackerController.profileList)
                if (item.playerName == selectedprofile)
                {
                    trackingenabled = item.tracked;
                    comparingenabled = item.compare;
                    break;
                }
        }

        [UIAction("apply-btn-action")]
        public void Apply()
        {
            ScoreTrackerController.ConfigVariable.SetBool("settings", "enabled", enabled);
        }
    }
}