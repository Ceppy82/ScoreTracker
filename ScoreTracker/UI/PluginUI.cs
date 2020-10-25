using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using ScoreTracker.UI.ViewController;
using System;
using System.Linq;
using UnityEngine;

namespace ScoreTracker.UI
{
    public class PluginUI : PersistentSingleton<PluginUI>
    {
        public MenuButton scoreTrackerButton;
        internal ScoreTrackerFlowCoordinator _scoreTrackerFlowCoordinator;
        public static GameObject _levelDetailClone;

        internal void Setup()
        {
            scoreTrackerButton = new MenuButton("ScoreTracker", "Track and Compare many Profiles", ButtonPressed, true);
            MenuButtons.instance.RegisterButton(scoreTrackerButton);
        }


        internal void ButtonPressed()
        {
            ShowScoreTrackerFlow();
        }

        internal void ShowScoreTrackerFlow()
        {
            _scoreTrackerFlowCoordinator = BeatSaberUI.CreateFlowCoordinator<ScoreTrackerFlowCoordinator>();
            _scoreTrackerFlowCoordinator.SetParentFlowCoordinator(BeatSaberMarkupLanguage.BeatSaberUI.MainFlowCoordinator);            
            BeatSaberMarkupLanguage.BeatSaberUI.MainFlowCoordinator.PresentFlowCoordinator(_scoreTrackerFlowCoordinator, null, HMUI.ViewController.AnimationDirection.Horizontal, true); // ("PresentFlowCoordinator", _moreSongsFlowCooridinator, null, false, false);
        }
    }
}
