using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ScallyWags
{
    public class HazardManager : MonoBehaviour
    {
        [SerializeField] private List<HazardData> _easyHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _mediumHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _hardHazards = new List<HazardData>();
        [SerializeField] private List<HazardData> _introduction = new List<HazardData>();
        [SerializeField] private HazardRating _currentHazardRating;

        private int _hazardsUnlocked = 1;
        private bool _introInProgress;

        [SerializeField] private float _hazardInterval = 10f;
        private float _hazardTimer = 0;

        private float _tutorialTimer = 0;
        private float _tutorialMaxDelay = 60f;
        
        private FloatVariable _roundTime;

        private bool _IsPaused;

        private enum HazardRating
        {
            Easy,
            Medium,
            Hard
        }

        private void OnEnable()
        {
            EventManager.StartListening("IntroDone", NextIntro);
            EventManager.StartListening("Pause", TogglePause);
        }

        private void TogglePause(EventManager.EventMessage arg0)
        {
            _IsPaused = !_IsPaused;
        }

        public void Init(FloatVariable roundTime)
        {
            _roundTime = roundTime;
            
            if (_easyHazards.Count == 0 || _mediumHazards.Count == 0 || _hardHazards.Count == 0 ||
                _introduction.Count == 0)
            {
                Debug.LogError("Missing hazards from lists");
            }
        }

        public void Tick()
        {
            if (_IsPaused) return;
            
            UpdateAvailableHazards();
            
            if (_easyHazards.Count == 0 && _mediumHazards.Count == 0 && _hardHazards.Count == 0 &&
                _introduction.Count == 0)
            {
                Debug.LogError("No hazards");
                return;
            }
            
            // Force progressing in tutorial to avoid hard locking the game
            _tutorialTimer += Time.deltaTime;
            if (_tutorialMaxDelay < _tutorialTimer)
            {
                _introInProgress = false;
                _tutorialTimer = 0;
            }

            _hazardTimer += Time.deltaTime;
            if (_hazardTimer < _hazardInterval) return;
            _hazardTimer = 0;
            
            if (_introduction.Count > 0)
            {
                TutorialHazard();
            }
            else
            {
                Hazard();
            }
        }

        private void TutorialHazard()
        {
            if (_introInProgress) return;
                
            SpawnIntroHazard();
            _introInProgress = true;

            if (_introduction.Count == 1)
            {
                _introInProgress = false;
            }
        }

        private void Hazard()
        {
            ChooseRating();
        }
        
        private void NextIntro(EventManager.EventMessage args)
        {
            _introInProgress = false;
            _tutorialTimer = 0;
        }

        private void UpdateAvailableHazards()
        {
            if (_roundTime.Value < 200 && _roundTime.Value > 100)
            {
                _hazardsUnlocked = 2;
            }

            if (_roundTime.Value < 100)
            {
                _hazardsUnlocked = 3;
            }
        }

        private void ChooseRating()
        {
            _currentHazardRating = (HazardRating) Random.Range(0, _hazardsUnlocked);

            switch (_currentHazardRating)
            {
                case HazardRating.Easy:
                    SpawnHazard(_easyHazards);
                    break;
                case HazardRating.Medium:
                    SpawnHazard(_mediumHazards);
                    break;
                case HazardRating.Hard:
                    SpawnHazard(_hardHazards);
                    break;
                default:
                    Debug.LogError("Hazard rating not set");
                    break;
            }
        }

        private void SpawnHazard(List<HazardData> hazards)
        {
            // If no hazards in this rating choose new hazard rating
            if (hazards.Count <= 0)
            {
                ChooseRating();
                return;
            }

            var index = Random.Range(0, hazards.Count);
            for(int i = 0; i < hazards[index].NumberOfHazards; i++)
            {
                EventManager.EventMessage eventMessage = new EventManager.EventMessage(hazards[index]);
                EventManager.TriggerEvent(hazards[index].EventName, eventMessage);   
            }
        }

        // Introduction hazards come one by one. Remove introduction hazards after triggering each hazard
        private void SpawnIntroHazard()
        {
            EventManager.EventMessage eventMessage = new EventManager.EventMessage(_introduction[0]);
            EventManager.TriggerEvent(_introduction[0].EventName, eventMessage);
            _introduction.RemoveAt(0);
        }
    }
}