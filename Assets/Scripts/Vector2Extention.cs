using UnityEngine;

namespace GraveDays.Utility {
	public static class Vector2Extension {

		public static Vector2 Rotate(this Vector2 v, float degrees) {
			float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
			float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

			float tx = v.x;
			float ty = v.y;
			Vector2 r = new Vector2 ();
			r.x = (cos * tx) - (sin * ty);
			r.y = (sin * tx) + (cos * ty);
			return r;
		}
	}
}