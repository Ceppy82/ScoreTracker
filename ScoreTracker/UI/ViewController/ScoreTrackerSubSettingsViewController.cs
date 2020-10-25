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
    public class ScoreTrackerSubSettingsViewController : BSMLResourceViewController
    {
        public override string ResourceName => "ScoreTracker.UI.BSML.subsettings.bsml";


        [UIValue("saved-profiles")]
        private List<object> savedprofiles = Tools.GetAllNames();


        [UIValue("tracking-enabled")]
        private bool trackingenabled = ScoreTrackerController.profileList[0].tracked;

        [UIValue("comparing-enabled")]
        private bool comparingenabled = ScoreTrackerController.profileList[0].compare;

        [UIAction("select-cover")]
        private void ShowModal()
        {
        }


        [UIValue("selected-profile")]
        private string selectedprofile = ScoreTrackerController.profileList[0].playerName;

        [UIAction("profile-changed")]
        public void ProfileChanged()
        {
            Plugin.Log?.Debug($"{name}: ProfileChanged");

            foreach (var item in ScoreTrackerController.profileList)
                if (item.playerName == selectedprofile)
                {
                    trackingenabled = item.tracked;
                    comparingenabled = item.compare;
                    break;
                }

        }





    }
}