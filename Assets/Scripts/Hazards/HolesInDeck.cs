using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using Random = UnityEngine.Random;

public class HolesInDeck : Hazard
{
    private int _minHoles;
    private int _maxHoles;
    private List<HoleInteraction> _holes = new List<HoleInteraction>();

    public override void Execute()
    {
        var holesToSpawn = Random.Range(_minHoles, _maxHoles+1);

        List<int> _holeIndexes = new List<int>();
        for (int i = 0; i < holesToSpawn; i++)
        {
            var index = Random.Range(0, _holes.Count);
            _holeIndexes.Add(index);
        }

        foreach (var hole in _holeIndexes)
        {
            SpawnHole(hole);
        }
    }

    private void SpawnHole(int index)
    {
        _holes[index].CreateHole();
    }
}
