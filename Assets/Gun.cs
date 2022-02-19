using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject Bullet;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(Bullet);
            bullet.transform.position = transform.position + transform.up * 0.1f;
            bullet.transform.up = transform.up;
        }
    }
}
