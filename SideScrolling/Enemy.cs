using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	public Image healthBar;
	public static float health;
	[SerializeField] float maxHealth = 200f; //We can manually adjust it's health with this
	private void Start() {
		healthBar = FindObjectOfType<Image>();
		health = maxHealth;
	}

	public void TakeDamage(float damage) //TakeDamage() methos so we can call it in other scripts
	{	
		healthBar.fillAmount = health / maxHealth;
		health -= damage; //health keeps decreasing
		

		if (health <= 0) //including < 0 bc some weapons can leave enemy at -x depending
                         //on it's attack
		{
			Die(); //Removes enemy once it's dead
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
