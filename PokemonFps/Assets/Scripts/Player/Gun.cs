using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
  public float damage;
  public float impactForce, fireRate;
  public Camera aimCam;
  public ParticleSystem flash;
  public GameObject effect;
  public GameObject effect2;

  float fireTime = 0f;
  void Update()
  {
    // !Kontroll
    if (Input.GetButton("Fire1") && Time.time >= fireTime)
    {
      fireTime = Time.time + 1f / fireRate;
      Shoot();
    }

  }
  void Shoot()
  {
    // !Skottet
    RaycastHit hit;
    if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit))
    {

      Target target = hit.transform.GetComponent<Target>();
      Debug.Log(hit.transform.name);

      // !Skadar Target
      if (target != null)
      {
        target.TakeDamage(damage);
        GameObject effektGO2 = Instantiate(effect2, hit.point, Quaternion.identity);
        Destroy(effektGO2, 1f);
        // Score.scoreValue += 20;
      }
      else
      {
        GameObject effektGO = Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(effektGO, 1f);
      }

      // !Force
      if (hit.rigidbody != null)
      {
        hit.rigidbody.AddForce(-hit.normal * impactForce);
      }
    }

    else
    {
      flash.Play();
    }
  }
}