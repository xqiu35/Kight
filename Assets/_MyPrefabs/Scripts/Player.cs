using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Config;
using RPG.Weapons;
using RPG.Core;
using RPG.Utils;
using RPG.Event;
using UnityEngine.SceneManagement;

namespace RPG.Characters
{
	[RequireComponent(typeof(AudioSource))]
	public class Player : Character, IDamageable
	{
		// ******************************************* Paras *******************************************
		PlayerMovement playerMovement = null;

		// ******************************************* Paras *******************************************
		float lastHitTime = 0f;
		bool canAttack = false;
		Enemy currentTarget = null;

		// ******************************************* Weapon *******************************************
		[Header("Character Weapon")]
		[SerializeField] Weapon weaponInUse = null;

		// ******************************************* Uity Calls ******************************************* 
		void Start ()
		{
			SetupCharacter ();
			useDefaultBaseValue ();
			SetupWeapon ();
			SetupRuntimeAnimator ();
			RegisterMouseEvent ();
			RegisterMovementEvents ();

			anim.runtimeAnimatorController = animatorOverrideController;
		}

		void Update () {
			AttackHandler ();
		}

		// ******************************************* Set ups *******************************************
		private void SetupRuntimeAnimator()
		{
			animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();
		}

		private void RegisterMouseEvent()
		{
			mouseEvent.onMouseOverWalkable += OnMouseOverWalkable;
			mouseEvent.onMouseOverEnemy += OnMouseOverEnemy;
		}

		private void RegisterMovementEvents()
		{
			playerMovement = GetComponent<PlayerMovement> ();
			playerMovement.onMovementStop += onMovementStop;
		}

		// ******************************************* Battle Calls ******************************************* 
		public void TakeDamage(int damage, float delay, AudioClip attackSound,GameObject attacker)
		{
			StartCoroutine (onDamage (damage, delay, attackSound,attacker));
		}

		IEnumerator onDamage(int damage, float delay,AudioClip attackSound,GameObject attacker)
		{
			c_health = Mathf.Clamp (c_health - damage, 0, health);
			yield return new WaitForSecondsRealtime (delay);
			UpdateHP ();
			audio.clip = attackSound;
			audio.Play ();
			if (c_health == 0)
			{
				StartCoroutine (Die());
			}
		}

		IEnumerator Die()
		{
			CharacterAnimatorPara.setDeath (anim, true);
			AudioClip[] clips = characterConfig.SoundClips;

			if (clips.Length > 0) {
				audio.clip = clips[PlayerSoundIndexes.Death];
				audio.Play();
				yield return new WaitForSecondsRealtime(audio.clip.length);
			}

			SceneManager.LoadScene(0);
		}

		// ******************************************* Equip Weapon ******************************************* 
		private void SetupWeapon()
		{
			GameObject weaponPrefab = weaponInUse.GetWeaponPrefab();
			GameObject dominantHand = RequestDominantHand();
			GameObject weapon = Instantiate(weaponPrefab, dominantHand.transform);
			weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
			weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
			animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();
			audio.clip = weaponInUse.GetAudioClip ();
		}

		private GameObject RequestDominantHand()
		{
			DominantHand dominantHand = GetComponentInChildren<DominantHand>();
			return dominantHand.gameObject;
		}

		// ******************************************* Attack ******************************************* 
		void OnMouseOverEnemy(Enemy enemy)
		{
			if (Input.GetMouseButtonDown ((int)attackConfig))
			{
				currentTarget = enemy;
				CheckAndSetCanAttack ();
			}
		}

		void OnMouseOverWalkable(Vector3 destination)
		{
			if (Input.GetMouseButtonDown ((int)attackConfig))
			{
				stopAttacking ();
			}
		}

		public bool isTargetInRange(Vector3 targetPos)
		{
			float dist = (targetPos - transform.position).magnitude;
			return dist <= c_attackRange;
		}

		void stopAttacking()
		{
			currentTarget = null;
			canAttack = false;
			anim.speed = playerMovement.moveAnimSpeed;
		}

		void AttackHandler()
		{
			if (currentTarget!=null && currentTarget.IsDead)
			{
				stopAttacking ();
			}
			if (canAttack)
			{		
				transform.LookAt (currentTarget.transform);

				if (currentTarget != null && Time.time - lastHitTime > 1f/c_attackSpeed)
				{
					anim.speed = attackSpeed;
					anim.SetTrigger (CharacterAnimatorPara.ATTACK);

					lastHitTime = Time.time;
					currentTarget.TakeDamage (c_attackDamage,weaponInUse.getDamageDelay(),weaponInUse.GetAudioClip(),gameObject);
				}
			}
		}

		void CheckAndSetCanAttack()
		{
			if (isTargetInRange (currentTarget.gameObject.transform.position))
			{
				canAttack = true;
				playerMovement.Stop ();
			}
			else
			{
				canAttack = false;
			}
		}

		void onMovementStop()
		{
			if (currentTarget != null) {
				canAttack = true;
			}
		}

		// ******************************** Getters *****************************************
		public bool getCanAttack()
		{
			return canAttack;
		}

		public bool isAttackCanceled()
		{
			return currentTarget == null;
		}

		// ******************************** Draw ********************************************
		void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere (transform.position, (float)c_attackRange);
		}
	}
}