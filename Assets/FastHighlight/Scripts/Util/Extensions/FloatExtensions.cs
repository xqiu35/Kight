using UnityEngine;
using System.Collections;

namespace Merlin
{
	public static class FloatExtensions 
	{
		private const float DefaultAcceptableDifference = 0.001f;

		public static bool ApproximatelyEquals(this float firstFloatValue, float secondFloatValue, float acceptableDifference = DefaultAcceptableDifference)
		{
			return Mathf.Abs(firstFloatValue - secondFloatValue) <= acceptableDifference;
		}
	}
}
