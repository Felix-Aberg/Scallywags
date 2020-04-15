using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [SerializeField] List<Hazard> _easyHazards = new List<Hazard>();
    [SerializeField] List<Hazard> _mediumHazards = new List<Hazard>();
    [SerializeField] List<Hazard> _hardHazards = new List<Hazard>();
    [SerializeField] List<Hazard> _introduction = new List<Hazard>();
    [SerializeField] private HazardRating _currentHazardRating;

    private float _hazardInterval = 10f;
    private float _timer = 0;

    private enum HazardRating
    {
        Easy,
        Medium,
        Hard
    }
    
    public void Init()
    {
        if (_easyHazards.Count == 0 || _mediumHazards.Count == 0 || _hardHazards.Count == 0 || _introduction.Count == 0)
        {
            Debug.LogError("Missing hazards from lists");
        }
    }

    public void Tick()
    {
        _timer += Time.deltaTime;

        if (_timer < _hazardInterval) return;

        return;
        
        if (_introduction.Count > 0)
        {
            SpawnIntroHazard(_introduction);
        }
        else
        {
            ChooseRating();
        }
    }
    
    private void ChooseRating()
    {
        _currentHazardRating = (HazardRating)Random.Range(0, 3);

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

    private void SpawnHazard(List<Hazard> hazards)
    {
        if (hazards.Count <= 0)
        {
            ChooseRating();
            return;
        }
        var index = Random.Range(0, hazards.Count);
        hazards[index].Execute();
    }

    private void SpawnIntroHazard(List<Hazard> introduction)
    {
        _introduction[0].Execute();
        _introduction.RemoveAt(0);
    }
}
