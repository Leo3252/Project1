using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSO : ScriptableObject
{
    public Sprite collectablePNG;
    public string value = "Type C for coin, H for health, D for damage increase";

    [Header("Give value for the type you chose above")]
    public int coin = 0;
    public int health = 0;
    public float damageIncrease = 0;
}
