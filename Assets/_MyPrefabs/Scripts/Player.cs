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
		Enemy targetEnemy;

		// ******************************************* Weapon *******************************************
		[Header("Character Weapon")]
		[SerializeField] Weapon weaponInUse = null;

		// ******************************************* Uity Calls ******************************************* 
		void Start () {
			anim = GetComponent<Animator> ();
			playerConfig = GetComponent<CharactorConfig> ();
			audio = GetComponent<AudioSource> ();
			mouseEvent = FindObjectOfType<MouseEvent> ();
			playerMovement = GetComponent<PlayerMovement> ();

			useDefaultBaseValue ();
			PutWeaponInHand ();
			SetupRuntimeAnimator ();
			RegisterForMouseClick ();
		}
		
		void Update () {
			AttackHandler ();
		}

		// ******************************************* Set ups *******************************************
		private void SetupRuntimeAnimator()
		{
			anim = GetComponent<Animator>();
			anim.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip();
		}

		private void RegisterForMouseClick()
		{
			mouseEvent.onMouseOverEnemy += OnMouseOverEnemy;
			mouseEvent.onMouseOverWalkable += OnMouseOverWalkable;
		}

		// ******************************************* Battle Calls ******************************************* 
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
			CharacterAnimatorPara.setDeath (anim, true);
			AudioClip[] clips = playerConfig.SoundClips;

			if (clips.Length > 0) {
				audio.clip = clips[PlayerSoundIndexes.Death];
				audio.Play();
				yield return new WaitForSecondsRealtime(audio.clip.length);
			}

			SceneManager.LoadScene(0);
		}

		// ******************************************* Equip Weapon ******************************************* 
		private void PutWeaponInHand()
		{
			var weaponPrefab = weaponInUse.GetWeaponPrefab();
			GameObject dominantHand = RequestDominantHand();
			var weapon = Instantiate(weaponPrefab, dominantHand.transform);
			weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
			weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
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
				print ("clicked");
				targetEnemy = enemy;

				if (isTargetInRange (enemy.gameObject.transform.position))
				{
					canAttack = true;
				}
				else
				{
					StartCoroutine (moveThenAttack ());
				}

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
			canAttack = false;
			targetEnemy = null;
			anim.speed = playerMovement.moveAnimSpeed;
		}

		void AttackHandler()
		{
			if (canAttack)
			{
				playerMovement.Stop ();
				transform.LookAt (targetEnemy.transform);
				if (targetEnemy != null && Time.time - lastHitTime > 1f/c_attackSpeed)
				{
					anim.speed = attackSpeed;
					anim.SetTrigger (CharacterAnimatorPara.ATTACK);

					targetEnemy.TakeDamage (c_attackDamage);
					lastHitTime = Time.time;
				}
			}
		}

		IEnumerator moveThenAttack()
		{
			while (!playerMovement.isStopped)
			{
				yield return new WaitForSeconds(0.01f);
			}
			canAttack = true;
		}

		// ******************************** Draw ********************************************
		void OnDrawGizmos()
		{
			Gizmos.DrawWireSphere (transform.position, (float)c_attackRange);
		}
	}
}
