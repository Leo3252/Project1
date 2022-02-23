using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //Using Unity's new Input System

public class SideScrolling : MonoBehaviour
{
    //public so we can refer directly to these scripts
    [Header("About Weapons and Effects")]
    public WeaponsSO weapon; //Allows me to reference with Inventory script and assign the weapon,
                             //public so it is accessable to other scripts
    
    public Transform firePos; //Allows me to get the transform of the gameObject and set it as
                              //firePos

    [HideInInspector]
    public CapsuleCollider2D playerCollider; //this is just for other scripts to reference is,
                                             //thus hiding from inspector

    //To access native components of this(player) gameObject
    Rigidbody2D rb;
    BoxCollider2D boxCollider;

    Inventory inventory; //reference to Inventory script

    bool m_FacingRight = true; //copied

    [Header("Player Settings")]
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] AudioClip _audio; // Muzzle Audio
    [SerializeField] ParticleSystem muzzleFlash; //Particle SYstem
    [SerializeField] GameObject impactEffect; //Particle system, set as gameobject
                                              //so I can instantiate
    [SerializeField] LineRenderer LineRenderer; //copied, but it's to create a "ray"
    [SerializeField] GameObject bullet; //allows me to instantiate this GameObject
    [SerializeField] Vector3 offSet;

    float fireRate = 0.5f;//working to apply to WeaponsSO
    float nextFire = 0f;
    
    Vector2 moveInput;

    void Start()
    {
        //Accessing components
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        transform.Rotate(0, 180, 0); //Calibrating player
        transform.Rotate(0, 180, 0);
        inventory = FindObjectOfType<Inventory>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        SpriteRenderer spriteRenderer;//To load the sprite of the current weapon we're holding
        //rememeber to transfer to OnEquip1/2
        spriteRenderer = GameObject.Find("Weapon").GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = weapon.weaponSprite;
        Run();
    }

    void OnMove(InputValue value) //New Input System
    {
        moveInput = value.Get<Vector2>(); //storing value
    }

    void OnJump(InputValue value)//Checking if space .isPressed and checking if box collider,
                                 //player foot is touching either the fllor or the enemy
    {
        if (value.isPressed && boxCollider.IsTouchingLayers(LayerMask.GetMask("Platforms", "Enemy")))
        {
            rb.velocity += new Vector2(0f, jumpHeight);
        }
    }

    void OnFire(InputValue value) //onLeftButtonMouseClick
    {
        if (weapon.weaponType == "Fire" && Time.time > nextFire) //Fire mode
        {
            
            nextFire = Time.time + fireRate; //Time.time is a timer that start when a scene is played,
                                             //so that + the fireRate and set equal to nextFire allow
                                             //us to enable fireRate
            AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position);
            muzzleFlash.Play();
            Instantiate(bullet, firePos.position, transform.rotation);

        } else if (weapon.weaponType == "Ray" && Time.time > nextFire) //ray mode
        {
            nextFire = Time.time + fireRate; //Working on change so that you hold mouse
            AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position);
            muzzleFlash.Play();
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot() //copied code xD, IEnumerator is the var for coroutines
    {  
        RaycastHit2D hitInfo = Physics2D.Raycast(firePos.position, firePos.right); //shooting a ray
        if (hitInfo) //on hit gives information if a *collider* is touched
        {
            GameObject cloneEffect = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(cloneEffect, 2f); //Intantiates the particle effect
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>(); //Accessing enemy script
            if (enemy != null) //if hitInfo on enemy is true and not null then 
            {       
                enemy.TakeDamage(weapon.damage);//Take damage      
            }
            LineRenderer.SetPosition(0, firePos.position + offSet); //Renders a line for
                                                                    //visual effects
            LineRenderer.SetPosition(1, hitInfo.point);
        } else
        {
            LineRenderer.SetPosition(0, firePos.position); //if it didn't hit anything
                                                           //it will render a line 100 units right
            LineRenderer.SetPosition(1, firePos.position + firePos.right * 100);
        }
        LineRenderer.enabled = true; 
        yield return new WaitForSecondsRealtime(0.02f); //Wait for 0.02 sec until disableling line
        LineRenderer.enabled = false;
    }

    void OnEquip1()//OnPlayerPress1
    {
        if (inventory.Winventory.Count == 0) {  return; } //if there is nothing in inventory
                                                          //break out of method
        if(weapon == inventory.Winventory[0]) { weapon = inventory.blank; } //If already holding
                                         //weapon then swap it to "universal blank" to "unequip"
        else { weapon = inventory.Winventory[0]; } //Finally if all those conditions are false
                                                   //equip weapon
    }
    void OnEquip2()//OnPlayerPress2
    {
        if (inventory.Winventory.Count == 0) { return; }//Same thing here but at index position 1
        if (weapon == inventory.Winventory[1]) { weapon = inventory.blank; }
        else { weapon = inventory.Winventory[1]; }

    }

    void Run()
    { 
        //x-axis is changing through moveInput var which returns 1 or -1, then that times player
        //speed, y-axis is left as it is
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, rb.velocity.y);
        rb.velocity = playerVelocity; //apply velocity
        if (rb.velocity.x > 0 && !m_FacingRight)
        {
            FlipSprite();
        }
        else if (rb.velocity.x < 0 && m_FacingRight)
        {
            FlipSprite();
        }
    }

    void FlipSprite() //copied
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
