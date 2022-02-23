using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponsSO : ScriptableObject
{
    [Header ("Weapon")]
    public Sprite weaponSprite;
    public Sprite ammoSprite;

    public string weaponName;
    public string ammoType;

    [Header("Weapon type")]
    public string weaponType = "Type in Fire or Ray";

    [Header ("Tweakings")]
    public float damage = 20f;
    public float range = 15f;
    public float bulletSpeed = 20f;

    [Header("Set pistol and fire position")]
    public Vector3 pistolPos = new Vector3(0, 0, 0);
    public Vector3 firePos = new Vector3(0, 0, 0);

}
