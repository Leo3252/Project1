using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayWeapon : MonoBehaviour
{

    Inventory inventory; SideScrolling player; //Variables to access other scripts,
                                               //inventory being reference point

    SpriteRenderer thisSpriteRenderer;//Variable to store native sprite renderer

    public WeaponsSO displayWeapon; //To directly access a WeaponsSO, easier to change 

    void Start()
    {
        thisSpriteRenderer = GetComponent<SpriteRenderer>(); //Accessing other scripts
        inventory = FindObjectOfType<Inventory>();
        player = FindObjectOfType<SideScrolling>();

        thisSpriteRenderer.sprite = displayWeapon.weaponSprite;//Access sprite renderer and
                                                               //applies WeaponsSO's sprite
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player") && //only player can "pick" it up
            player.playerCollider.IsTouchingLayers(LayerMask.GetMask("Weapon"))) //assure it can
                                                                                 //only be collected
                                                                                 //once
        {
            if (inventory.Winventory.Count < 2) // Maximum inventory is set to 2,
                                                // bc index position is is less than 2,
                                                // being 0, 1
            {
                inventory.Winventory.Add(displayWeapon); //Adding the picked up weapon to the
                                                         //reference point "Inventory"
                Destroy(gameObject);
            }
        }
    }

}
