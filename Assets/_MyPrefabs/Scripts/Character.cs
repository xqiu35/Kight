using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Config;
using RPG.Event;
using RPG.Utils;

namespace RPG.Characters
{
	[RequireComponent(typeof(CharactorConfig))]
	public class Character : MonoBehaviour
	{
		// ******************************************* Properties *******************************************
		[Header("Character Base")]
		[SerializeField] protected int level;
		[SerializeField] protected int health;
		[SerializeField] protected int energy;
		[SerializeField] protected int attackDamage;
		[SerializeField] protected int magicPower;
		[SerializeField] protected int armor;
		[SerializeField] protected float attackSpeed;
		[SerializeField] protected float attackRange;
		[SerializeField] protected int criticalHit;

		[Header("Character Current")]
		[SerializeField] protected int c_level;
		[SerializeField] protected int c_health;
		[SerializeField] protected int c_energy;
		[SerializeField] protected int c_attackDamage;
		[SerializeField] protected int c_magicPower;
		[SerializeField] protected int c_armor;
		[SerializeField] protected float c_attackSpeed;
		[SerializeField] protected float c_attackRange;
		[SerializeField] protected int c_criticalHit;

		[SerializeField] protected AttackConfig attackConfig;
		[SerializeField] protected AnimatorOverrideController animatorOverrideController;

		// ******************************************* Para & Component *******************************************
		protected Animator anim;
		protected CharactorConfig characterConfig;
		protected AudioSource audio;
		protected MouseEvent mouseEvent;


		// ******************************************* Start *******************************************
		protected void SetupCharacter()
		{
			anim = GetComponent<Animator> ();
			characterConfig = GetComponent<CharactorConfig> ();
			audio = GetComponent<AudioSource> ();
			mouseEvent = FindObjectOfType<MouseEvent> ();
		}

		// ******************************************* UI Calls ******************************************* 
		public float healthAsPercentage { get { return (float)c_health / (float)health; } }
		public float energyAsPercentage { get { return (float)c_energy / (float)energy; } }

		// ******************************************* Test Calls *******************************************
		protected void useDefaultBaseValue()
		{
			level = 1;
			health = 100;
			energy = 100;
			attackDamage = 10;
			magicPower = 50;
			armor = 50;
			attackSpeed = 1;
			attackRange = 1.5f;
			criticalHit = 10;

			c_level = level;
			c_health = health;
			c_energy = energy;
			c_attackDamage = attackDamage;
			c_magicPower = magicPower;
			c_armor = armor;
			c_attackSpeed = attackSpeed;
			c_attackRange = attackRange;
			c_criticalHit = criticalHit;
		}

		// ******************************************* UI Calls ******************************************* 
		protected void UpdateHP()
		{
			if (characterConfig==null || characterConfig.HealthOrb == null) {
				return;
			}
			characterConfig.HealthOrb.fillAmount = healthAsPercentage;
		}

		protected void UpdateMP()
		{
			if (characterConfig==null || characterConfig.HealthOrb == null) {
				return;
			}
			characterConfig.EnergyOrb.fillAmount = energyAsPercentage;
		}

		// ******************************************* Getters ******************************************* 
		public float getCurrentAttackRange()
		{
			return c_attackRange;
		}
	}
}
