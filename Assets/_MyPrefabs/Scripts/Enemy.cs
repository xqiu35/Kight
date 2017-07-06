using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO consider re-wire
using RPG.Core;
using RPG.Weapons;
using RPG.Event;
using RPG.Config;
using RPG.Utils;
using UnityEngine.UI;

namespace RPG.Characters
{
	public class Enemy : Character,IDamageable
    {
		// ******************************************* Paras *******************************************
		bool isDead = false;
		RawImage healthBar;
		Player player;

		// ******************************************* Unity Calls *******************************************
		void Start()
		{
			SetupCharacter ();
			anim.runtimeAnimatorController = animatorOverrideController;
			player = FindObjectOfType<Player> ();

			useDefaultBaseValue ();
			SetupRuntimeAnimator ();
		}

		void Update()
		{
		}

		// ******************************************* Setups *******************************************
		private void SetupRuntimeAnimator()
		{
			anim.runtimeAnimatorController = animatorOverrideController;
		}

		// ******************************************* Damage Taken *******************************************
		public void TakeDamage(int damage, float delay, AudioClip attackSound,GameObject attacker)
		{
			StartCoroutine (onDamage (damage, delay,attackSound,attacker));
		}

		IEnumerator onDamage(int damage, float delay, AudioClip attackSound, GameObject attacker)
		{
			yield return new WaitForSecondsRealtime (delay);

			if(attacker!=null && attacker.GetComponent<Player>()!=null)
			{
				if (!player.isAttackCanceled())
				{
					if (isLastHit(damage))
					{
						audio.clip = characterConfig.SoundClips[(int)(UnityEngine.Random.Range(0, characterConfig.SoundClips.Length))];
					}
					else
					{
						audio.clip = attackSound;
					}

					c_health = Mathf.Clamp (c_health - damage, 0, health);

					UpdateHealth ();

					audio.Play ();
				}
			}
				
			if (c_health == 0)
			{
				StartCoroutine (Die());
			}
		}
			
		IEnumerator Die()
		{
			isDead = true;
			animatorOverrideController ["DEFAULT DEATH"] = characterConfig.DeathAnimations [(int)(UnityEngine.Random.Range(0, characterConfig.DeathAnimations.Length))];
			anim.SetTrigger(CharacterAnimatorPara.DEATH);

			/*float clipLength = characterConfig.SoundClips.Length;
			audio.clip = characterConfig.SoundClips[(int)(UnityEngine.Random.Range(0, clipLength))];
			audio.Play();*/
			yield return new WaitForSecondsRealtime(0f);
		}

		bool isLastHit(int damage)
		{
			return (c_health - damage <= 0);
		}

		// ******************************************* Getters Setters *******************************************
		public bool IsDead{get{return isDead;}set{isDead = value;}}


		// ******************************************* Update UI *******************************************
		void UpdateHealth()
		{
			if (healthBar == null) {
				healthBar = GetComponentInChildren<RawImage> ();
			}
			float xValue = -(healthAsPercentage / 2f) - 0.5f;

			healthBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
		}
    }
}