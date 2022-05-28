using System.Collections;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class GunfireController : MonoBehaviour
    {
        // --- Audio ---
        public AudioClip GunShotClip;
        public AudioSource source;
        public Vector2 audioPitch = new Vector2(.9f, 1.1f);

        // --- Muzzle ---
        public GameObject muzzlePrefab;
        public GameObject muzzlePosition;

        // --- Config ---
        public bool autoFire;
        public float shotDelay = .5f;
        public bool rotate = true;
        public float rotationSpeed = .25f;

        // --- Options ---
        public GameObject scope;
        public bool scopeActive = true;
        private bool lastScopeState;

        public float damage;
        public float impactForce, fireRate;
        public Camera aimCam;
        public GameObject effect;
        public GameObject effect2;
        public TrailRenderer BulletTrail;

        float fireTime = 0f;
        // --- Projectile ---
        [Tooltip("The projectile gameobject to instantiate each time the weapon is fired.")]
        public GameObject projectilePrefab;
        [Tooltip("Sometimes a mesh will want to be disabled on fire. For example: when a rocket is fired, we instantiate a new rocket, and disable" +
            " the visible rocket attached to the rocket launcher")]
        public GameObject projectileToDisableOnFire;

        // --- Timing ---
        [SerializeField] private float timeLastFired;


        private void Start()
        {
            if(source != null) source.clip = GunShotClip;
            timeLastFired = 0;
            lastScopeState = scopeActive;
        }

        private void Update()
        {
            // --- If rotate is set to true, rotate the weapon in scene ---
            if (rotate)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y 
                                                                        + rotationSpeed, transform.localEulerAngles.z);
            }

            if (Input.GetMouseButton(0) && Time.time >= fireTime)
            {
                fireTime = Time.time + 1f / fireRate;
                FireWeapon();
            }

            // --- Toggle scope based on public variable value ---
            if(scope && lastScopeState != scopeActive)
            {
                lastScopeState = scopeActive;
                scope.SetActive(scopeActive);
            }
        }

        public void FireWeapon()
        {
            // --- Keep track of when the weapon is being fired ---
            timeLastFired = Time.time;

            // --- Spawn muzzle flash ---
            var flash = Instantiate(muzzlePrefab, muzzlePosition.transform);

            // --- Disable any gameobjects, if needed ---
            if (projectileToDisableOnFire != null)
            {
                projectileToDisableOnFire.SetActive(false);
                Invoke("ReEnableDisabledProjectile", 3);
            }

            // --- Handle Audio ---
            if (source != null)
            {
                // --- Sometimes the source is not attached to the weapon for easy instantiation on quick firing weapons like machineguns, 
                // so that each shot gets its own audio source, but sometimes it's fine to use just 1 source. We don't want to instantiate 
                // the parent gameobject or the program will get stuck in a loop, so we check to see if the source is a child object ---
                if(source.transform.IsChildOf(transform))
                {
                    source.Play();
                }
                else
                {
                    // --- Instantiate prefab for audio, delete after a few seconds ---
                    AudioSource newAS = Instantiate(source);
                    if ((newAS = Instantiate(source)) != null && newAS.outputAudioMixerGroup != null && newAS.outputAudioMixerGroup.audioMixer != null)
                    {
                        // --- Change pitch to give variation to repeated shots ---
                        newAS.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", Random.Range(audioPitch.x, audioPitch.y));
                        newAS.pitch = Random.Range(audioPitch.x, audioPitch.y);

                        // --- Play the gunshot sound ---
                        newAS.PlayOneShot(GunShotClip);

                        // --- Remove after a few seconds. Test script only. When using in project I recommend using an object pool ---
                        Destroy(newAS.gameObject, 3);
                    }
                }
            }

                        // --- Shoot Projectile Object ---
                GameObject bullet = Instantiate(projectilePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation, transform);

                bullet.transform.position = muzzlePosition.transform.position;

                var rot = bullet.transform.rotation.eulerAngles;
                float bulletSpeed = 20;

                bullet.transform.rotation = Quaternion.Euler(rot.x, transform.eulerAngles.y, rot.z);

                bullet.GetComponent<Rigidbody>().AddForce(aimCam.transform.position * bulletSpeed, ForceMode.Impulse);

                Destroy(bullet, 3);


                RaycastHit hit;
                Vector3 direction = GetDirection();

                if (Physics.Raycast(aimCam.transform.position, aimCam.transform.forward, out hit))
                {

                Target target = hit.transform.GetComponent<Target>();
                Debug.Log(hit.transform.name);
                
                // !Trail
                TrailRenderer trail = Instantiate(BulletTrail, muzzlePosition.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
                Destroy(trail, 1f);
                
                // !Skadar Target
                if (target != null)
                {
                    target.TakeDamage(damage);
                    GameObject effektGO2 = Instantiate(effect2, hit.point, Quaternion.identity);
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

        }

    private Vector3 GetDirection()
    {
        Vector3 direction = aimCam.transform.forward;
        Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
        bool AddBulletSpread = true;

        if (AddBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)
            );

            direction.Normalize();
        }

        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }
        // Animator.SetBool("IsShooting", false);
        Trail.transform.position = Hit.point;

        Destroy(Trail.gameObject, Trail.time);
    }

    private void ReEnableDisabledProjectile()
    {
        projectileToDisableOnFire.SetActive(true);
    }

    }
}