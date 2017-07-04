using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
	[CreateAssetMenu(menuName = ("RPG/Weapon"))]
	public class Weapon : ScriptableObject
	{
		public Transform gripTransform;

		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;


		[SerializeField] int level;
		[SerializeField] int health;
		[SerializeField] int energy;
		[SerializeField] int attackDamage;
		[SerializeField] int magicPower;
		[SerializeField] int armor;
		[SerializeField] int attackSpeed;
		[SerializeField] int attackRange;
		[SerializeField] int criticalHit; // persentage

		public float GetMinTimeBetweenHits()
		{
			return 1f/(float)attackSpeed;
		}

		public float GetAttackRange()
		{
			return attackRange;
		}

		public GameObject GetWeaponPrefab()
		{
			return weaponPrefab;
		}

		public AnimationClip GetAttackAnimClip()
		{
			RemoveAnimationEvents();
			return attackAnimation;
		}

		// So that asset packs cannot cause crashes
		private void RemoveAnimationEvents()
		{
			if (attackAnimation.events.Length>0)
			{
				attackAnimation.events = null;
			}
		}
	}
}
