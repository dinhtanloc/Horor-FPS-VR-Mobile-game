using UnityEngine;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    //public GameObject impactEffect;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }
    public void Shoot()
    {
        muzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            // Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            //GameObject ImpactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(ImpactGo, 2f);
        }
    }
}