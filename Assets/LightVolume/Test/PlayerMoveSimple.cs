using System;
using UnityEngine;

namespace DefaultNamespace
{
	public class PlayerMoveSimple : MonoBehaviour
	{
		public float speed = 5;
		private void Update()
		{
			if (Input.GetKey(KeyCode.A))
			{
				transform.Translate(speed * Time.deltaTime * Vector3.left);
			}
			
			if (Input.GetKey(KeyCode.D))
			{
				transform.Translate(Time.deltaTime * speed * Vector3.right);
			}
			
			if (Input.GetKey(KeyCode.W))
			{
				transform.Translate(Time.deltaTime * speed * Vector3.up);
			}
			
			if (Input.GetKey(KeyCode.S))
			{
				transform.Translate(Time.deltaTime * speed * Vector3.down);
			}
		}
	}
}