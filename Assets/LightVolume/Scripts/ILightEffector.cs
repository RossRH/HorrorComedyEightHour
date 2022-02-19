using System;

namespace LightVolume
{
	public interface ILightEffector
	{
		void RegisterLight(Light2D light);

		void UnRegisterLight(Light2D light);

		void DirtyLights();
	}
}

