using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{ 
    [CreateAssetMenu(menuName="UsableItems")]
    public class UsableItems : ScriptableObject
    { 
        public List<PickableItem> itemList = new List<PickableItem>();
    } 
}