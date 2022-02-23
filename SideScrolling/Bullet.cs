using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //Initializing variables 

    Rigidbody2D rb; //Access self rigidbody

    SideScrolling player; //Access other scripts
    Enemy enemy;

    float speed;//speed of bullet
    float dmg;//damage of bullet

    float startPos;//These are to apply a range on certain weapons
    float currentPos;
    float distanceTravelled;
    
    bool isRight;//used later to apply velocity

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 

        player = FindObjectOfType<SideScrolling>();
        enemy = FindObjectOfType<Enemy>();

        CheckRotation();

        speed = player.weapon.bulletSpeed; //Declaring speed and damage of bullet through player's
        dmg = player.weapon.damage;        //current weapon
    }

    void CheckRotation()
    {
        //Because SideScrolling (player) scipt rotates the player on the y-axis by -180º to
        //flip the sprites instead of changing scale to -1 everything is rotated thus
        //using rotation check to see if it's going left or right.
        if (transform.rotation.y >= 0) 
        {
            isRight = true; //Goes right
        }
        else if (transform.rotation.y < 0)
        {
            isRight = false; //Goes left
        }
    }

    private void Awake()
    {  
        startPos = transform.position.x; //Holding it's initial position, when it is instantiated
    }

    private void Update()
    {
        currentPos = transform.position.x; //Updating position every frame
        distanceTravelled = currentPos - startPos; //Calculating distance travelled to apply range

        if (isRight) //using the previous CheckRotation
        {
            rb.velocity = new Vector2(speed, 0f); //Applying a positive acceleration
                                                  //on x-axis so it moves to the right
        } else if (!isRight)
        {
            rb.velocity = new Vector2(-speed, 0f); //Applying a negative acceleration
                                                   //on x-axis so it moves to the left
        }

        if (distanceTravelled > player.weapon.range || distanceTravelled < -(player.weapon.range)) 
        {
            Destroy(gameObject); //applying range
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy") //Enemies with enemy tag will be affected 
        {       
            enemy.TakeDamage(dmg); //Accessing the Enemy script to take health away from enemy
        }
        Destroy(gameObject); //Remember to remove the bullet too
    }

    private void OnCollisionEnter(Collision other) //on collision removes bullet from scene
    {
        Destroy(gameObject);
    }

}
