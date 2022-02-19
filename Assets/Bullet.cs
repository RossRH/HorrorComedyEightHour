using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	
	public LayerMask WaterLayerMask;
    public float Velocity;

    public float DestroyTime;

    public GameObject WaterSplatCreator;

    public void Start()
    {
	    GetComponent<Rigidbody2D>().velocity = transform.up * Velocity;
    }
    
    public void OnCollisionEnter2D(Collision2D other)
    {
	    if (WaterLayerMask == (WaterLayerMask | (1 << other.gameObject.layer)))
	    {
		    GameObject waterSplatCreator = Instantiate(WaterSplatCreator);
		    waterSplatCreator.transform.position = transform.position;
		    waterSplatCreator.GetComponent<WaterSplatCreator>().Normal = other.contacts[0].normal;
	    }

	    StartCoroutine(DestroyAfter(DestroyTime));
    }

    public IEnumerator DestroyAfter(float seconds)
    {
	    yield return new WaitForSeconds(seconds);
	    Destroy(gameObject);
    }

    public void OnDestroy()
    {
	    
    }
}
