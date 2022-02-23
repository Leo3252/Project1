using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //Reference point
    public List<WeaponsSO> Winventory;
    public List<CollectableSO> Cinventory;
    //Because player without a SO it will give an error there is a universal blank SO carrying
    //no information
    public WeaponsSO blank;

}
