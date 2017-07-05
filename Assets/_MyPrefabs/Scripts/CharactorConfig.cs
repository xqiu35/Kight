using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Config
{
	public class CharactorConfig : MonoBehaviour
	{
		[Header("Character UI")]
		[SerializeField] Image healthOrb;
		[SerializeField] Image energyOrb;

		[Header("Character Animations")]
		[SerializeField] AnimationClip[] deathAnimations;
		[SerializeField] AnimationClip[] postAnimations;

		[Header("Character Sounds")]
		[SerializeField] AudioClip[] soundClips;

		public Image HealthOrb {
			get{ return healthOrb; }
			set{ }
		}

		public Image EnergyOrb {
			get{ return energyOrb; }
			set{ }
		}

		public AnimationClip[] DeathAnimations {
			get{ return deathAnimations; }
			private set{ }
		}

		public AnimationClip[] PostAnimations {
			get{ return postAnimations; }
			private set{ }
		}

		public AudioClip[] SoundClips {
			get{ return soundClips; }
			private set{ }
		}
	}
}
