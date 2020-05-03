using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

namespace ScallyWags
{
    public class LevelEventManager : MonoBehaviour
    {
        public enum LevelPlayState
        {
            InProgress,
            Won,
            Lost,
            Skip,
            Quit
        }

        private Scene thisScene;
        private LevelPlayState state = LevelPlayState.InProgress;
        private int score = 0;
        private float secondsElapsed = 0;
        private int deaths = 0;

        void Awake()
        {
            thisScene = SceneManager.GetActiveScene();
            AnalyticsEvent.LevelStart(thisScene.name,
                thisScene.buildIndex);
            
            UpdateScore(100);
            
            AnalyticsEvent.TutorialStart();
            EventManager.StartListening("Intro1", TutorialOne);
            EventManager.StartListening("Intro2", TutorialTwo);
            EventManager.StartListening("Intro3", TutorialThree);
            EventManager.StartListening("KrakenIntro", TutorialComplete);
        }

        private void TutorialOne(EventManager.EventMessage msg)
        {
            AnalyticsEvent.TutorialStep(1);
        }
        
        private void TutorialTwo(EventManager.EventMessage msg)
        {
            AnalyticsEvent.TutorialStep(2);
        }
        
        private void TutorialThree(EventManager.EventMessage msg)
        {
            AnalyticsEvent.TutorialStep(3);
        }
        
        private void TutorialComplete(EventManager.EventMessage arg0)
        {
            AnalyticsEvent.TutorialComplete();
        }

        public void SetLevelPlayState(LevelPlayState newState)
        {
            this.state = newState;
        }

        public void UpdateScore(int points)
        {
            score = points;
        }

        public void IncrementDeaths()
        {
            deaths++;
        }

        void Update()
        {
            secondsElapsed += Time.deltaTime;
        }

        void OnDestroy()
        {
            Dictionary<string, object> customParams = new Dictionary<string, object>();
            customParams.Add("seconds_played", secondsElapsed);
            customParams.Add("points", score);
            customParams.Add("deaths", deaths);

            switch (this.state)
            {
                case LevelPlayState.Won:
                    AnalyticsEvent.LevelComplete(thisScene.name,
                        thisScene.buildIndex,
                        customParams);
                    break;
                case LevelPlayState.Lost:
                    AnalyticsEvent.LevelFail(thisScene.name,
                        thisScene.buildIndex,
                        customParams);
                    break;
                case LevelPlayState.Skip:
                    AnalyticsEvent.LevelSkip(thisScene.name,
                        thisScene.buildIndex,
                        customParams);
                    break;
                case LevelPlayState.InProgress:
                case LevelPlayState.Quit:
                default:
                    AnalyticsEvent.LevelQuit(thisScene.name,
                        thisScene.buildIndex,
                        customParams);
                    break;
            }
        }
    }
}
