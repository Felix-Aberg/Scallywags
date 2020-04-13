using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public interface IEntity
{
    void Init(int index = 0);
    void Tick();
}
