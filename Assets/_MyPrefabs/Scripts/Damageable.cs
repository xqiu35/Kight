using System.Collections;

namespace RPG.Core
{
	public interface IDamageable
	{
		void TakeDamage(int damage,float delay);
	}
}