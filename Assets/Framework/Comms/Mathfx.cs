using UnityEngine;
using System.Collections;

public class Mathfx
{		
		// 艾米插值
		public static Vector3 Hermite(Vector3 start, Vector3 end, float value)
		{
				return new Vector3(Hermite(start.x, end.x, value), Hermite(start.y, end.y, value), Hermite(start.z, end.z, value));
		}

		public static float Hermite(float start, float end, float value)
		{
				return Mathf.Lerp(start, end, value * value * (3.0f-2.0f*value));
		}
}

