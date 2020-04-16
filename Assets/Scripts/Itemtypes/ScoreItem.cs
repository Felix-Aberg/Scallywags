using ScallyWags;
using UnityEngine;

namespace ScallyWags
{
    
public class ScoreItem : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _dividesTo;
    public int GoldValue => _goldValue;
    [SerializeField] private int _goldValue;
    [SerializeField] private int maxHitpoints = 2;
    private int _hitPoint;
    private float _distance = 1f;
    private TreasureManager _treasureManager;
    private CreateUIElement _createUIElement;
    private bool _lost;

    public void Init(TreasureManager treasureManager)
    {
        _createUIElement = FindObjectOfType<CreateUIElement>();
        _treasureManager = treasureManager;
        _hitPoint = maxHitpoints;
    }
    
    public void TakeDamage()
    {
        _hitPoint--;
        if (_hitPoint <= 0)
        {
            if (_dividesTo != null)
            {
                Divide();
            }
        }
    }

    public void Tick()
    {
        if (_lost) return;
        
        if (transform.position.y < -15f)
        {
            _lost = true;
            _goldValue = 0;
            _createUIElement.CreateElement(UIElement.GoldLost, transform.position);
            _treasureManager.ReCalculateGold();
        }
    }

    private void Divide()
    {
        SpawnItem(_dividesTo);
        SpawnItem(_dividesTo);
        _goldValue = 0;
        gameObject.SetActive(false);
        _treasureManager.ReCalculateGold();
    }

    private void SpawnItem(GameObject bagOfGold)
    {
        var item = Instantiate(bagOfGold, GetRandomPos(), Quaternion.identity);
        item.GetComponent<ScoreItem>().Init(_treasureManager);
    }

    private Vector3 GetRandomPos()
    {
        var random = UnityEngine.Random.insideUnitCircle * _distance;
        var pos = transform.position + new Vector3(random.x, 0, random.y);
        return pos;
    }
}

}
