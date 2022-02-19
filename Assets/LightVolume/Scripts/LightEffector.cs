using System.Collections.Generic;
using UnityEngine;

namespace LightVolume
{
	public class LightEffector : MonoBehaviour, ILightEffector
	{
		private List<Light2D> lights = new List<Light2D>();

		public void RegisterLight(Light2D light) {
			lights.Add (light);
		}

		public void UnRegisterLight(Light2D light) {
			lights.Remove (light);
		}

		public void DirtyLights() {
			foreach (Light2D light in lights) {
				light.MarkDirty ();
			}
		}
	}
}

