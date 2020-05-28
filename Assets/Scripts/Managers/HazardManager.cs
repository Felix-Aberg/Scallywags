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
        [SerializeField] private HazardRating _currentHazardRating = HazardRating.Easy;

        [Header("Used to spawn ship at the start of the game")]
        [SerializeField] private HazardData _enemyShip;

        [SerializeField] private HazardData _krakens;

        private ShipCondition _playerShip;
        private RoundTimer _roundTimer;

        private int _hazardsUnlocked = 1;
        private bool _introInProgress;

        [SerializeField] private float _hazardInterval = 10f;
        private float _hazardTimer = 0;

        private float _tutorialTimer = 0;
        private float _tutorialMaxDelay = 180f;
        
        private FloatVariable _roundTime;
        private PlayersSelected _players;
        
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

        public void Init(RoundTimer roundTimer, FloatVariable roundTime, ShipCondition ship, PlayersSelected players)
        {
            _players = players;
            _roundTimer = roundTimer;
            _roundTime = roundTime;
            _playerShip = ship;
            
            if (_easyHazards.Count == 0 || _mediumHazards.Count == 0 || _hardHazards.Count == 0 ||
                _introduction.Count == 0)
            {
                Debug.LogError("Missing hazards from lists");
            }
            
            EventManager.TriggerEvent("SpawnEnemyShip", new EventManager.EventMessage(_enemyShip));
            EventManager.TriggerEvent("CreateKrakens", new EventManager.EventMessage(_krakens));
        }

        public void Tick()
        {
            UpdateDifficulty();
            
            if (_playerShip.IsSinking())
            {
                _IsPaused = true;
            }
            
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
                _roundTimer.BeginRound();
                EventManager.TriggerEvent("protectTreasure", null);
                Hazard();
            }
        }

        private void UpdateDifficulty()
        {
            switch (_players.GetPlayersReady())
            {
                case 1:
                    _hazardInterval = 15f;
                    break;
                case 2:
                    _hazardInterval = 12.5f;
                    break;
                case 3:
                    _hazardInterval = 10f;
                    break;
                case 4:
                    _hazardInterval = 8.5f;
                    break;
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
            // unlock medium hazards when 1/3 of the round has passed
            if (_roundTime.Value < _roundTimer.RoundTime/3 && _roundTime.Value > _roundTimer.RoundTime/3*2)
            {
                _hazardsUnlocked = 1;
            }

            // unlock hard hazards when 2/3 of the round has passed
            if (_roundTime.Value < _roundTimer.RoundTime/3*2)
            {
                _hazardsUnlocked = 2;
            }
        }

        private void ChooseRating()
        {
            _currentHazardRating = (HazardRating) _hazardsUnlocked;

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

        public void SkipHazard()
        {
            #if UNITY_EDITOR
            Debug.Log("Skipped hazard");
            #endif
            _hazardTimer = _hazardInterval;
        }
    }
}