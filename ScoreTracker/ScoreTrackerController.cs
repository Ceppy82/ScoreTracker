using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BS_Utils;
using BS_Utils.Utilities;
using BS_Utils.Gameplay;
using IPA.Utilities;
using System.IO;
using TMPro;
using UnityEngine.UI;
using BeatSaberMarkupLanguage.GameplaySetup;
using HMUI;
using BeatSaberMarkupLanguage;
using ScoreTracker.UI;

namespace ScoreTracker
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class ScoreTrackerController : MonoBehaviour
    {
        public static ScoreTrackerController Instance { get; private set; }

        public static BS_Utils.Utilities.Config ConfigVariable = new BS_Utils.Utilities.Config("ScoreTracker");
        public static string ScoreTrackerDB;
        public static int delayCounter = 0;
        public static List<Classes.Playerinfo> profileList = new List<Classes.Playerinfo>();
        public static List<Classes.Tracked> trackedList = new List<Classes.Tracked>();


        public void InitiateScoreTracker()
        {
            Plugin.Log?.Debug($"{name}: InitiateScoreTracker()");

            //check first run
            if (ConfigVariable.GetString("settings", "path for Database") == "")
            {
                ConfigVariable.SetBool("settings", "enabled", true);
                ConfigVariable.SetString("settings", "path for Database", UnityGame.UserDataPath + "\\ScoreTracker\\ScoreTracker.db");
                ConfigVariable.SetString("inject new profile(s)", "ScoreSaberID(s)", "https://scoresaber.com/u/76561197963480061");
                Directory.CreateDirectory(UnityGame.UserDataPath + "\\ScoreTracker");                
            }
            ScoreTrackerDB = ConfigVariable.GetString("settings", "path for Database");
        }

        public void UpdateScoreTracker()
        {

            Plugin.Log?.Debug($"{name}: UpdateScoreTracker()");
            List<string> _newIDlist = new List<string>();
            if (ConfigVariable.GetString("inject new profile(s)", "ScoreSaberID(s)") != "")
            {
                var _newIDs = ConfigVariable.GetString("inject new profile(s)", "ScoreSaberID(s)");
                ConfigVariable.SetString("inject new profile(s)", "ScoreSaberID(s)", "");   
                _newIDlist = Tools.GetAllIDs(_newIDs);
                Plugin.Log?.Debug($"{name}: injected ID(s): {_newIDlist}");
            }
            foreach (var item in _newIDlist)
            {
                Tools.Delayed("GetScoreSaberData", item);
            }
            profileList = FromDB.GetProfilesAsList();
                foreach (var item in profileList)
            {
                Tools.Delayed("UpdateScoreSaberData", item.playerId);
            }
            List<string> _tracked = new List<string>();           
            _tracked = FromDB.GetTrackedFromDB();
            //initiate tracking    
            foreach (var item in _tracked)
                {
                Tools.Delayed("UpdateScoreSaberData", item);
                foreach (var item2 in profileList)
                {
                    if (item == item2.playerId)
                    {
                        trackedList.Add(new Classes.Tracked(item2.timestampu,
                            item2.playerId, item2.playerName, item2.pp,
                            item2.rank, item2.countryRank,
                            item2.country, item2.timestamp));
                    }
                }
            }        
        }






        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        /// 
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            if (Instance != null)
            {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            InitiateScoreTracker();
            GameObject.DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;
            Plugin.Log?.Debug($"{name}: Awake()");
    }
    /// <summary>
    /// Only ever called once on the first frame the script is Enabled. Start is called after any other script's Awake() and before Update().
    /// </summary>
    private void Start()
        {

            BSEvents.menuSceneActive -= UpdateScoreTracker;
            BSEvents.menuSceneActive += UpdateScoreTracker;
        }

        /// <summary>
        /// Called every frame if the script is enabled.
        /// </summary>
        private void Update()
        {

        }

        /// <summary>
        /// Called every frame after every other enabled script's Update().
        /// </summary>
        private void LateUpdate()
        {

        }

        /// <summary>
        /// Called when the script becomes enabled and active
        /// </summary>
        private void OnEnable()
        {


            GameObject canvas = new GameObject("canvas", typeof(RectTransform));
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<GraphicRaycaster>();
            GameObject button = new GameObject("button", typeof(RectTransform));
            button.AddComponent<Button>();
            button.AddComponent<CanvasRenderer>();
            button.AddComponent<Image>();
            button.transform.parent = canvas.transform;
            PluginUI.instance.Setup();

        }

        /// <summary>
        /// Called when the script becomes disabled or when it is being destroyed.
        /// </summary>
        private void OnDisable()
        {
            BSEvents.menuSceneActive -= UpdateScoreTracker;

        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            BSEvents.menuSceneActive -= UpdateScoreTracker;
            ToDB.SaveProfiles();
            ToDB.SaveTracked();
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.

        }


        #endregion



    }
}
