using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.ViewControllers;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using ScoreTracker.UI.ViewController;
using HMUI;
using System;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UI;
using BeatSaberUI = BeatSaberMarkupLanguage.BeatSaberUI;



namespace ScoreTracker.UI
{
    public class ScoreTrackerFlowCoordinator : FlowCoordinator
    {
        public FlowCoordinator ParentFlowCoordinator { get; protected set; }
        public bool AllowFlowCoordinatorChange { get; protected set; } = true;
        private NavigationController _scoreTrackerNavigationcontroller;
        private ScoreTrackerSettingsViewController _scoreTrackerSettingsView;
        private ScoreTrackerSubSettingsViewController _scoreTrackerSubSettingsView;





        public void Awake()
        {
            if (_scoreTrackerSettingsView == null)
            {
                _scoreTrackerSettingsView = BeatSaberUI.CreateViewController<ScoreTrackerSettingsViewController>();
                _scoreTrackerNavigationcontroller = BeatSaberUI.CreateViewController<NavigationController>();
            }
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
       
                if (firstActivation)
                {
                    SetTitle("ScoreTracker");

                    showBackButton = true;

                    SetViewControllersToNavigationController(_scoreTrackerNavigationcontroller, _scoreTrackerSettingsView);
                ProvideInitialViewControllers(_scoreTrackerNavigationcontroller, _scoreTrackerSettingsView);

                //  PopViewControllerFromNavigationController(_moreSongsNavigationcontroller);
            }

        }
        public void SetParentFlowCoordinator(FlowCoordinator parent)
        {
            if (!AllowFlowCoordinatorChange)
                throw new InvalidOperationException("Changing the parent FlowCoordinator is not allowed on this instance.");
            ParentFlowCoordinator = parent;
        }

        protected override void BackButtonWasPressed(HMUI.ViewController topViewController)
        {
            BeatSaberUI.MainFlowCoordinator.DismissFlowCoordinator(this);
            base.BackButtonWasPressed(topViewController);
        }
    }
}