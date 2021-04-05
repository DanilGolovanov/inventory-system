using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Object", menuName = "Inventory System/Items/Potion")]
public class PotionObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Potion;
    }
}
