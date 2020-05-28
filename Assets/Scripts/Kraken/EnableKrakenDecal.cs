using System.Collections;
using System.Collections.Generic;
using kTools.Decals;
using UnityEngine;

namespace ScallyWags
{
    public class EnableKrakenDecal : MonoBehaviour
{
    private Decal _decal;
    [SerializeField] float _telegraphDuration = 1f;

    void Start()
    {
        _decal = GetComponent<Decal>();
        _decal.enabled = false;
    }

    public void EnableDecal()
    {
        if (_decal.enabled)
        {
            return;
        }
        _decal.enabled = true;
        StartCoroutine(Wait());
    }

    public void DisableDecal()
    {
        _decal.enabled = false;
    }
    
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_telegraphDuration);
        DisableDecal();

    }
}
}