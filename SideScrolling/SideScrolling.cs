using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SideScrolling : MonoBehaviour
{
    public WeaponsSO weapon;
    public CapsuleCollider2D playerCollider;
    public Transform firePos;
    
    Rigidbody2D rb;
    BoxCollider2D boxCollider;
    Inventory inventory;

    private bool m_FacingRight = true;

    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float jumpHeight = 10f;


    float damage;

    [SerializeField] AudioClip _audio;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;
    [SerializeField] LineRenderer LineRenderer;
    [SerializeField] GameObject bullet;

    [SerializeField] Vector3 offSet;

    float fireRate = 0.5f;//working to apply to WeaponsSO
    float nextFire = 0f;
    
    Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        transform.Rotate(0, 180, 0);
        transform.Rotate(0, 180, 0);
        inventory = FindObjectOfType<Inventory>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        SpriteRenderer spriteRenderer;
        spriteRenderer = GameObject.Find("Weapon").GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = weapon.weaponSprite;
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && boxCollider.IsTouchingLayers(LayerMask.GetMask("Platforms", "Enemy")))
        {
            rb.velocity += new Vector2(0f, jumpHeight);
        }
    }

    void OnFire(InputValue value)
    {
        

        if (weapon.weaponType == "Fire" && Time.time > nextFire)
        {
            
            nextFire = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position);
            muzzleFlash.Play();
            Instantiate(bullet, firePos.position, transform.rotation);

        } else if (weapon.weaponType == "Ray" && Time.time > nextFire)
        {
            damage = weapon.damage;
            nextFire = Time.time + fireRate;
            AudioSource.PlayClipAtPoint(_audio, Camera.main.transform.position);
            muzzleFlash.Play();
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {  
        RaycastHit2D hitInfo = Physics2D.Raycast(firePos.position, firePos.right);
        if (hitInfo)
        {
            GameObject cloneEffect = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(cloneEffect, 2f);
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {       
                enemy.TakeDamage(damage);      
            }
            LineRenderer.SetPosition(0, firePos.position + offSet);
            LineRenderer.SetPosition(1, hitInfo.point);
        } else
        {
            LineRenderer.SetPosition(0, firePos.position);
            LineRenderer.SetPosition(1, firePos.position + firePos.right * 100);
        }
        LineRenderer.enabled = true;
        yield return new WaitForSecondsRealtime(0.02f);
        LineRenderer.enabled = false;
    }

    void OnEquip1()
    {
        if (inventory.Winventory.Count == 0) {  return; }
        if(weapon == inventory.Winventory[0]) { weapon = inventory.blank; }
        else { weapon = inventory.Winventory[0]; }

    }
    void OnEquip2()
    {
        if (inventory.Winventory.Count == 0) { return; }
        if (weapon == inventory.Winventory[1]) { weapon = inventory.blank; }
        else { weapon = inventory.Winventory[1]; }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        if (rb.velocity.x > 0 && !m_FacingRight)
        {
            FlipSprite();
        }
        else if (rb.velocity.x < 0 && m_FacingRight)
        {
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


}
