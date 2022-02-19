using LightVolume;
using UnityEngine;

namespace GraveDays.Entity.Management {
	public class VisColliderScanner : MonoBehaviour {
		public LayerMask mainLayerMask;

		private Collider2D[] mainColliders;

		private Vector2 lastPos;
		[SerializeField]
		private VisibilityField2D visField;

		private Camera mainCam;

		// Use this for initialization
		void Awake () {
			mainColliders = new Collider2D[220];
			mainCam = Camera.main;
		}
		
		// Update is called once per frame
		void FixedUpdate () {

			if (mainCam == null)
			{
				return;
			}

			if (lastPos.Equals((Vector2)transform.position)) {
				// when re-adding this, sort out doors to trigger update when opening or closing
				
				//	return;
			}
			
			float vertExtent = 2.35f * mainCam.orthographicSize;    
			float horzExtent = vertExtent * Screen.width / Screen.height;
			Vector2 newSize = new Vector2 (horzExtent, vertExtent);

			int mainCollidersCount = Physics2D.OverlapBoxNonAlloc(transform.position + new Vector3(0 -1.5f, 0) * 0, newSize, 0, mainColliders, mainLayerMask.value);
			
			visField.SetNewVisibilityFieldBoundaries (mainColliders, mainCollidersCount);

			lastPos = transform.position;
		}

	}
}