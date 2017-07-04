using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO consider re-wire
using RPG.Core;
using RPG.Weapons;
using RPG.Event;

namespace RPG.Characters
{
	public class Enemy : Character,IDamageable
    {
		void Start()
		{
			useDefaultBaseValue ();
		}

		void Update()
		{
		}

		public void TakeDamage(int damage)
		{
			c_health = Mathf.Clamp (c_health - damage, 0, health);
			UpdateHP ();
			if (c_health == 0) {
				StartCoroutine (Die());
			}
		}
			
		IEnumerator Die()
		{
			//return null;
			yield return new WaitForSecondsRealtime(0.1f);
		}
    }
}