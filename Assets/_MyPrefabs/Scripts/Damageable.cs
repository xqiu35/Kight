using System.Collections;
using UnityEngine;

namespace RPG.Core
{
	public interface IDamageable
	{
		void TakeDamage (int damage, float delay, AudioClip attackSound,GameObject attacker);
	}
}