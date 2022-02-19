using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplat : MonoBehaviour
{
    public float Lifetime;
    
    // Start is called before the first frame update
    void Start()
    {

       StartCoroutine(DestroyAfter(Lifetime));
    }
    
    void Update()
    {
        
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
