using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Rendering;

public class PostProcessVisibility : MonoBehaviour {

	public Material material;
	public static Material postProcessMatCopy;
	
	public Camera entityCam;

	// cam for entities that are always visible (even in vis field)
	public Camera visEntityCam;
	public Camera lightCam;
	public Camera visCam;

	private int width;
	private int height;

	private float redFactor;


	private float sicknessIntensity = 0;
	private float sicknessSpeed = 0;


	private CommandBuffer _commandBuffer;



	public float dayValue;
	public Color dayColor;
	
	// Use this for initialization
	void Awake ()
	{
		postProcessMatCopy = new Material(material);
		lightCam.allowHDR = true;
	}
	
	
	

	// Update is called once per frame
	void Update () {
		postProcessMatCopy.SetFloat ("_DayLight", dayValue);
		postProcessMatCopy.SetColor("_DayColor", dayColor);

		//postProcessMatCopy.SetFloat ("_DayLightColorNet", 0);


		

		postProcessMatCopy.SetFloat("_SicknessIntensity", 0);
		postProcessMatCopy.SetFloat("_SicknessSpeed", 0);
		//postProcessMatCopy.SetFloat("_NightVisionStrength", 0);

		
		postProcessMatCopy.SetFloat("_RedFactor", 0);


		if (width != (int)(Screen.width * entityCam.rect.width) || height != (int)(Screen.height * entityCam.rect.height)) {
			width = (int)(Screen.width * entityCam.rect.width);
			height = (int)(Screen.height * entityCam.rect.height);

			if (entityCam.targetTexture != null) {
				entityCam.targetTexture.Release ();
			}

			entityCam.targetTexture = new CustomRenderTexture (width, height);
			postProcessMatCopy.SetTexture ("_EntityTex", entityCam.targetTexture);

			if (visEntityCam.targetTexture != null) {
				visEntityCam.targetTexture.Release ();
			}

			visEntityCam.targetTexture = new CustomRenderTexture (width, height);
			postProcessMatCopy.SetTexture ("_VisEntityTex", visEntityCam.targetTexture);

			if (visCam.targetTexture != null) {
				visCam.targetTexture.Release ();
			}

			visCam.targetTexture = new CustomRenderTexture (width, height);
			postProcessMatCopy.SetTexture ("_VisTex", visCam.targetTexture);

			if (lightCam.targetTexture != null) {
				lightCam.targetTexture.Release ();
			}

			lightCam.targetTexture = new CustomRenderTexture (width, height);
			postProcessMatCopy.SetTexture ("_LightTex0", lightCam.targetTexture);
			
		}
		
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {


		Graphics.Blit(src, dest, postProcessMatCopy);
	}
}
