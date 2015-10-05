using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

    public float fireRate;
    public Bullet bullet;

    Transform barrel;

    bool flipImage = false;

    void Start()
    {
        barrel = transform.FindChild("Barrel");
    }
    void Update ()
    {
	    
	}

    public void FireBullet()
    {
        Instantiate(bullet, barrel.position, barrel.rotation);
    }
}
