using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace LightVolume {
	public class VisibilityField2D : MonoBehaviour {

		private MeshFilter meshFilter;
		private Mesh mesh;
		private MeshRenderer meshRenderer;

		public bool IsStatic = false;

		private GameObject visibilityMesh;
		public Material visibilityMat;
		private MeshCollider meshCollider;

		void Awake() {
			visibilityMesh = new GameObject ();
			visibilityMesh.transform.parent = transform;
			visibilityMesh.layer = gameObject.layer;
			meshRenderer = visibilityMesh.AddComponent<MeshRenderer> ();
			meshRenderer.material = visibilityMat;
			meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
			meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
			meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			meshFilter = visibilityMesh.AddComponent<MeshFilter> ();
			meshCollider = visibilityMesh.AddComponent<MeshCollider>();
			Rigidbody body = visibilityMesh.AddComponent<Rigidbody>();
			body.isKinematic = false;
			body.detectCollisions = false;
			body.useGravity = false;
		}

		float vertExtent;    
		float horzExtent;
		float aspectAngle;

		bool initialised = false;



		// Use this for initialization
		void DoStart () {
			if (initialised) {
				DestroyImmediate (mesh);
			}

			initialised = true;
			mustReInit = false;


			mesh = new Mesh ();
			meshFilter.mesh = mesh;
			mesh.MarkDynamic ();

			/*Camera mainCam = GetComponent<Camera>();
			vertExtent = mainCam.orthographicSize;    
			horzExtent = vertExtent * mainCam.pixelWidth / mainCam.pixelHeight;
			Vector2 camSize = new Vector2 (horzExtent, vertExtent) * 2.05f;


			aspectAngle = Vector2.Angle (Vector2.up, new Vector2 (horzExtent, vertExtent));*/

		//	Debug.Log ("DRAW LIGHT " + gameObject.name + " " + rayCount + " " + raysToDo.Count);
				
		}


		HashSet<GameObject> visibilityHits = new HashSet<GameObject> ();

		private Collider2D[] mainColliders;
		private int mainCollidersCount;

		private List<Vector2> points = new List<Vector2> ();
		private List<Vector2> boxPoints = new List<Vector2> ();


		public void SetNewVisibilityFieldBoundaries(Collider2D[] mainColliders, int mainColliderCount) {
			this.mainColliders = mainColliders;
			this.mainCollidersCount = mainColliderCount;
		}

		/*Vector2 clampScreenRay(Vector2 ray) {

			float theta = Vector2.Angle (Vector2.up, ray);

			float maxL = Mathf.Abs (theta) < aspectAngle || (Mathf.Abs (theta - 180) < aspectAngle) ?
				Mathf.Abs (vertExtent / Mathf.Cos (theta * Mathf.Deg2Rad)) :
				Mathf.Abs (horzExtent / Mathf.Cos ((theta - 90) * Mathf.Deg2Rad));

			return Vector2.ClampMagnitude (ray, maxL);
		}*/


		private List<Vector2> uvs = new List<Vector2> ();
		private List<Vector3> lightMeshBounds3D = new List<Vector3> ();
		private List<int> triangles = new List<int> ();
		
		private List<Vector2> extendedPoints = new List<Vector2>();


		private void UpdateField() {
			uvs.Clear ();
			lightMeshBounds3D.Clear ();
			triangles.Clear ();


			Vector2 centerPoint = transform.position;

			if (mainColliders != null) {
				int triCount = 0;
				for (int i = 0; i < mainCollidersCount; i++) {
					if (!mainColliders[i].enabled)
					{
						continue;
					}
					
					BoxCollider2D b = mainColliders[i] as BoxCollider2D;

					if (b == null)
					{
						continue;
					}
					
					boxPoints.Clear ();
					extendedPoints.Clear ();

					float top = b.offset.y + (b.size.y / 2f);
					float btm = b.offset.y - (b.size.y / 2f);
					float left = b.offset.x - (b.size.x / 2f);
					float right = b.offset.x + (b.size.x / 2f);

					Vector2 localP = b.transform.InverseTransformPoint (centerPoint);

					//if (localP.x < left || localP.y < btm) {
						boxPoints.Add (b.transform.TransformPoint (new Vector3 (left, btm, 0f)));
					//}

					//if (localP.x < left || localP.y > top) {
						boxPoints.Add (b.transform.TransformPoint (new Vector3 (left, top, 0f)));
					//}

					//if (localP.x > right || localP.y > top) {
						boxPoints.Add (b.transform.TransformPoint (new Vector3 (right, top, 0f)));
					//}
					//if (localP.x > right || localP.y < btm) {
						boxPoints.Add (b.transform.TransformPoint (new Vector3 (right, btm, 0f)));
					//}


					//TRI 4
					lightMeshBounds3D.Add (boxPoints[0] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[1] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[3] - centerPoint);

					triangles.Add (triCount * 3);
					triangles.Add (triCount * 3 + 1);
					triangles.Add (triCount * 3 + 2);
					triCount++;

					//TRI 5
					lightMeshBounds3D.Add (boxPoints[3] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[1] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[2] - centerPoint);

					triangles.Add (triCount * 3);
					triangles.Add (triCount * 3 + 1);
					triangles.Add (triCount * 3 + 2);
					triCount++;

					if (localP.y >= top) {
					//	boxPoints = boxPoints.OrderBy (p => Vector2.SignedAngle (p - centerPoint, Vector2.down)).ToList ();
					}
					else {
					//	boxPoints = boxPoints.OrderBy (p => Vector2.SignedAngle (p - centerPoint, Vector2.up)).ToList ();
					}

					Vector2 sortDir = localP.y >= top ? Vector2.down : Vector2.up;
					for (int m = 0; m < boxPoints.Count - 1; m++)
					{
						for (int n = m + 1; n < boxPoints.Count; n++)
						{
							Vector2 p0 = boxPoints[m];
							Vector2 p1 = boxPoints[n];
							if (Vector2.SignedAngle(p0 - centerPoint, sortDir) >
							    Vector2.SignedAngle(p1 - centerPoint, sortDir))
							{
								var tempP = boxPoints[n];
								boxPoints[n] = boxPoints[m];
								boxPoints[m] = tempP;
							}
						}
					}
					

					{
						Vector2 p = boxPoints[0];
						Vector2 ray = (p - (Vector2)centerPoint).normalized;
						Vector2 extendedP = p + ray * 25;
						extendedPoints.Add (extendedP);


						p = Vector2.Lerp(boxPoints[1], boxPoints[2], 0.5f);
						ray = (p - (Vector2)centerPoint).normalized;
						extendedP = p + ray * 25;
						extendedPoints.Add (extendedP);


						p = boxPoints[3];
						ray = (p - (Vector2)centerPoint).normalized;
						extendedP = p + ray * 25;
						extendedPoints.Add (extendedP);
					}

					//TRI 1
					lightMeshBounds3D.Add (extendedPoints[0] - centerPoint);
					lightMeshBounds3D.Add (extendedPoints[1] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[0] - centerPoint);

					triangles.Add (triCount * 3);
					triangles.Add (triCount * 3 + 1);
					triangles.Add (triCount * 3 + 2);
					triCount++;

					//TRI 2
					lightMeshBounds3D.Add (extendedPoints[1] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[0] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[3] - centerPoint);

					triangles.Add (triCount * 3);
					triangles.Add (triCount * 3 + 1);
					triangles.Add (triCount * 3 + 2);
					triCount++;

					//TRI 3
					lightMeshBounds3D.Add (extendedPoints[1] - centerPoint);
					lightMeshBounds3D.Add (extendedPoints[2] - centerPoint);
					lightMeshBounds3D.Add (boxPoints[3] - centerPoint);

					triangles.Add (triCount * 3);
					triangles.Add (triCount * 3 + 1);
					triangles.Add (triCount * 3 + 2);
					triCount++;

				}
			}


			mesh.Clear ();
			
			mesh.SetVertices(lightMeshBounds3D);
			mesh.SetTriangles(triangles, 0, false);
			//MeshUtility.Optimize(mesh);
			
			//mesh.vertices = lightMeshBounds3D.ToArray ();
			//mesh.triangles = triangles.ToArray ();

			visibilityMesh.transform.localEulerAngles = -transform.eulerAngles;
			visibilityMesh.transform.position = transform.position;

		}

		

		private void TagVisibleVisibilitySprites() {
	
		}

		bool mustReInit = false;
		public void MarkDirty() {
			dirty = true;
			mustReInit = true;
		}


		// Update is called once per frame
		bool dirty = true;
		private Vector3 lastPos;
		void Update () {
			if (IsStatic && lastPos != transform.position) {
				mustReInit = true;
			}

			if (!initialised || mustReInit) {
				DoStart ();
				dirty = true;
				mustReInit = false;
			}

			UpdateField();

			lastPos = transform.position;
		}

	}
}
