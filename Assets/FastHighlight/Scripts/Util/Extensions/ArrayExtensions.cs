using UnityEngine;
using System;

namespace Merlin
{
	public static class ArrayExtensions 
	{
		public static bool Exists<T>(this T[] array, Predicate<T> match)
		{
			if(array == null) 
			{
                throw new ArgumentNullException("array");
            }
 
            if(match == null) 
            {
                throw new ArgumentNullException("match");
            }

			for (int i = 0; i < array.Length; i++) 
			{
                if(match(array[i])) return true;
            }

            return false;
		}
	}
}