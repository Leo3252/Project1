using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] float health = 100; //We can manually adjust it's health with this

	public void TakeDamage(float damage) //TakeDamage() methos so we can call it in other scripts
	{
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
