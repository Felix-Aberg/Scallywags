using UnityEngine;

public class ScoreItem : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _bagOfGold;
    [SerializeField] private int maxHitpoints = 2;
    private int hitPoint;
    [SerializeField] private int goldValue;
    private float _distance = 1f;
    
    private void Start()
    {
        hitPoint = maxHitpoints;
    }
    
    public void TakeDamage()
    {
        hitPoint--;
        if (hitPoint <= 0)
        {
            Divide();
        }
    }

    private void Divide()
    {
        Instantiate(_bagOfGold, GetRandomPos(), Quaternion.identity);
        Instantiate(_bagOfGold, GetRandomPos(), Quaternion.identity);
        Destroy(gameObject);
    }

    private Vector3 GetRandomPos()
    {
        var random = UnityEngine.Random.insideUnitCircle * _distance;
        var pos = transform.position + new Vector3(random.x, 0, random.y);
        return pos;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        TakeDamage();
    }
}
