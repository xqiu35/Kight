using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Utils
{
	public enum ClickMoveConfig {LEFT_CLICK_MOVE,RIGHT_CLIVK_MOVE}
	public enum AttackConfig {LEFT_CLICK_ATTACK,RIGHT_CLICK_ATTACK}

	public class CharacterAnimatorPara
	{
		public const string IDLE = "Idle";
		public const string RUN = "Run";
		public const string DEATH = "Death";
		public const string ATTACK = "Attack";

		public static void setIdle(Animator anim, bool value)
		{
			anim.SetBool (IDLE, value);
			anim.SetBool (RUN, false);
			anim.SetBool (DEATH, false);
			anim.SetBool (ATTACK, false);
		}

		public static void setWalk(Animator anim, bool value)
		{
			anim.SetBool (RUN, value);
			anim.SetBool (DEATH, false);
			anim.SetBool (ATTACK, false);
			anim.SetBool (IDLE, false);
		}

		public static void setAttack(Animator anim, bool value)
		{
			anim.SetBool (RUN, false);
			anim.SetBool (DEATH, false);
			anim.SetBool (ATTACK, value);
			anim.SetBool (IDLE, false);
		}

		public static void setDeath(Animator anim, bool value)
		{
			anim.SetBool (RUN, false);
			anim.SetBool (DEATH, value);
			anim.SetBool (ATTACK, false);
			anim.SetBool (IDLE, false);
		}
	}

	public class PlayerSoundIndexes
	{
		public const int Death = 0;
	}

	public class EnvParas
	{
		public const int WALKABLE_LAYER = 8;
	}
}
