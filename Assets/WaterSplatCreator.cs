using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplatCreator : MonoBehaviour
{
    public GameObject WaterSplatPrefab;

    public Vector3 Normal;

    public float SplatInitialVelocity;

    IEnumerator Start()
    {
        StartCoroutine(DestroyAfter(5f));
        while (this != null)
        {
            yield return new WaitForSeconds(0.5f);
            CreateSplat();
        }
	 
    }

    // Update is called once per frame
    void CreateSplat()
    {
        GameObject splat = Instantiate(WaterSplatPrefab);
        splat.transform.position = transform.position;
        splat.GetComponent<Rigidbody2D>().velocity = Normal * SplatInitialVelocity;
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
