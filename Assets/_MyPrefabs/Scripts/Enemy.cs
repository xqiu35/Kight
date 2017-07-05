using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO consider re-wire
using RPG.Core;
using RPG.Weapons;
using RPG.Event;
using RPG.Config;

namespace RPG.Characters
{
	public class Enemy : Character,IDamageable
    {
		void Start()
		{
			playerConfig = GetComponent<CharactorConfig> ();
			useDefaultBaseValue ();
		}

		void Update()
		{
		}

		public void TakeDamage(int damage, float delay)
		{
			StartCoroutine (onDamage (damage, delay));
		}

		IEnumerator onDamage(int damage, float delay)
		{
			c_health = Mathf.Clamp (c_health - damage, 0, health);
			yield return new WaitForSecondsRealtime (delay);
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