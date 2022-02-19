using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GraveDays.Utility;
using UnityEngine;

namespace LightVolume {
	[System.Serializable]
	public class Light2D : MonoBehaviour {

		[SerializeField]
		private MeshFilter meshFilter;
		[SerializeField]
		private Mesh mesh;
		[SerializeField]
		private MeshRenderer meshRenderer;
		[SerializeField]
		public LayerMask lightAffectorLayer;

		[SerializeField]
		public float angle = 360;
		[SerializeField]
		public float rayCount = 500;
		[SerializeField]
		public float rayMaxLength = 25;
		[SerializeField]
		[ColorUsageAttribute(false,true)]
		public Color color;
		[SerializeField]
		private float _intensity = 1;

		public float Intensity {
			get {
				return _intensity;
			}

			set {
				_intensity = value;
				dirty = true;
				SwitchLight(_intensity > 0);
			}
		}

		[SerializeField]
		public bool IsStatic = false;

		List<Vector2> raysToDo = new List<Vector2> ();

		[SerializeField]
		public GameObject lightMesh;

		[SerializeField]
		private bool mustDoAwake = true;

		void Awake() {
			
			// for JSON template
			if (mustDoAwake)
			{
				lightMesh = new GameObject();
				lightMesh.transform.parent = transform;
				lightMesh.layer = gameObject.layer;
				meshRenderer = lightMesh.AddComponent<MeshRenderer>();
				meshRenderer.material = GetComponent<MeshRenderer>().material;
				meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
				meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
				meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
				meshRenderer.receiveShadows = false;
				Destroy(GetComponent<MeshRenderer>());
				meshFilter = lightMesh.AddComponent<MeshFilter>();
				IsOn = true;

				mustDoAwake = false;
			}
		}

		private bool initialised = false;

		private List<ILightEffector> lightAffectors = new List<ILightEffector>();

		// Use this for initialization
		void DoStart () {
			if (initialised) {
				DestroyImmediate (mesh);
				raysToDo.Clear ();

				foreach (ILightEffector a in lightAffectors) {
					a.UnRegisterLight (this);
				}

				lightAffectors.Clear ();
			}

			if (IsStatic) {
				Collider2D[] affectors = Physics2D.OverlapCircleAll (transform.position, rayMaxLength, lightAffectorLayer.value);

				foreach (Collider2D a in affectors) {
					ILightEffector lightEffector = a.GetComponent<ILightEffector> ();

					if (lightEffector != null) {
						lightAffectors.Add (lightEffector);
						lightEffector.RegisterLight (this);
					}
				}
			}

			initialised = true;
			mustReInit = false;


			mesh = new Mesh ();
			meshFilter.mesh = mesh;
			mesh.MarkDynamic ();
				

			// TODO: Scale rays to screen size
			for (int i = 0; i <= (int)rayCount; i++) {
				float a = i / rayCount * angle - angle/2.0f;
				float x = Mathf.Sin (Mathf.Deg2Rad * a) * rayMaxLength;
				float y = Mathf.Cos (Mathf.Deg2Rad * a) * rayMaxLength;


				Vector2 ray = new Vector2 (x, y);



				raysToDo.Add (ray);
			}

		//	Debug.Log ("DRAW LIGHT " + gameObject.name + " " + rayCount + " " + raysToDo.Count);
				
		}

		public bool IsOn {
			get;
			private set;
		}

		private List<Vector2> uvs = new List<Vector2> ();
		private List<Vector3> lightMeshBounds3D = new List<Vector3> ();
		private List<int> triangles = new List<int> ();


		private void UpdateLightOld() {
			
			float appliedIntensity = (Intensity + 0.3f) * 1.2f;

			if (!IsOn) {
				appliedIntensity = 0;
			}

			float maxRay = rayMaxLength * (Intensity + 0.05f);

			Vector2 centerPoint = transform.position;

		//	Debug.Log ("DRAW LIGHT " + gameObject.name + " " + raysToDo.Count);



			uvs.Clear ();
			lightMeshBounds3D.Clear ();
			triangles.Clear ();

			lightMeshBounds3D.Add (Vector2.zero);
			uvs.Add (Vector2.one * 0.5f);


			int vertCount = 1;

			Collider2D lastHit = null;
			for (int i = 0; i < raysToDo.Count; i+=1) {
				Vector2 ray = raysToDo [i];

				if (IsStatic) {

				}
				else {
					ray = ray.Rotate (transform.eulerAngles.z);
				}
					
				RaycastHit2D h = new RaycastHit2D();
				if (IsStatic) {
					h = Physics2D.Raycast (centerPoint, ray, ray.magnitude, 1 << 13);
				}
				if (h.collider != null) {
					Vector2 p = h.point;// + r / maxRay * 0.3f;
					lightMeshBounds3D.Add (p - centerPoint);
					uvs.Add (((Vector2)p - centerPoint) / maxRay * 0.5f + Vector2.one * 0.5f);
				}
				else {
					lightMeshBounds3D.Add (ray);
					uvs.Add (ray / maxRay * 0.5f + Vector2.one * 0.5f);
				}

				if (vertCount > 1) {
					triangles.Add (vertCount);
					triangles.Add (0);
					triangles.Add (vertCount-1);
				}
				vertCount++;
			}

			triangles.Add (1);
			triangles.Add (0);
			triangles.Add (vertCount-1);

		/*	if (IsVisibility) {
				foreach (Vector2 v in lightMeshBounds) {
					Debug.DrawLine (centerPoint, v + centerPoint, Color.green);
					Debug.DrawLine (v + centerPoint, v * 2 + centerPoint, Color.red);
				}
			}*/
				

			mesh.Clear ();
			mesh.vertices = lightMeshBounds3D.ToArray ();
			mesh.triangles = triangles.ToArray ();
			mesh.SetUVs (0, uvs);

			lightMesh.transform.localEulerAngles = -transform.eulerAngles;
			lightMesh.transform.position = transform.position;

			//Debug.Log("LIGHT: " + color )

			meshRenderer.material.SetColor ("_Tint", color);
			meshRenderer.material.SetFloat ("_Intensity", appliedIntensity);

		}

		private void TagVisibleVisibilitySprites() {
	
		}

		bool mustReInit = false;
		public void MarkDirty() {
			dirty = true;
			mustReInit = true;
			
			Debug.Log("MARK DIRTY");
		}


		// Update is called once per frame
		bool dirty = true;
		private Vector3 lastPos;
		void Update () {
			if (IsStatic && lastPos != transform.position) {
				mustReInit = true;
			}
			
			lastPos = transform.position;

			if (!initialised || mustReInit) {
				DoStart ();
				dirty = true;
				mustReInit = false;
				UpdateLightOld ();
			}
			
			float appliedIntensity = (Intensity + 0.3f) * 1.2f;

			if (!IsOn) {
				appliedIntensity = 0;
			}

			meshRenderer.material.SetColor ("_Tint", color);
			meshRenderer.material.SetFloat ("_Intensity", appliedIntensity);
			
			if (!IsOn) {
				return;
			}


			if (dirty && IsStatic) {
				UpdateLightOld ();
			
				dirty = false;
			}




			if (!IsStatic) {
				dirty = true;
			}



			

		}

		public void SwitchLight(bool enabled) {
			if (IsOn == enabled) {
				return;
			}

			IsOn = enabled;
			dirty = true;
		}
	}
}
