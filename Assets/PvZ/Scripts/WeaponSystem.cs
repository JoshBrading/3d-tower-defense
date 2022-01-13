using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    AudioSource audioData;

    [Header("Basic")]
    public float damage;
    public float range, reloadTime, timeBetweenBurstShots;
    public LayerMask targetLayer;

    [Header("Advanced")]
    [Tooltip("Adds 1 projectile to positive and negative x")]
    public float tileSpread;
    public int shotsPerBurst = 1;
    public Vector3 viewPosOffset;
    private bool ready = true, reloading;

    [Header("Projectile")]
    public bool isProjectile;
    public Vector3 angularVelocity = new Vector3(1, 0, 0);

    public Vector3 projectileScale;
    public GameObject projectile;

    void Start()
    {
        audioData = GetComponent<AudioSource>();

    }
    public void TriggerAttack( GameObject target )
    {
        if (ready)
        {
            StartCoroutine(Attack( target ));
        }
    }

    IEnumerator Attack(GameObject target)
    {
        ready = false;

        for (int i = 0; i < shotsPerBurst; i++)
        {
            if( isProjectile)
            {
                GameObject s_Projectile = Instantiate(projectile, this.transform.position + viewPosOffset, Quaternion.identity);
                var proj_think = s_Projectile.GetComponent<ProjectileThink>();
                proj_think.angularVelocity = angularVelocity;
                proj_think.damage = damage;
                proj_think.lifetime = range;

                s_Projectile.transform.localScale = projectileScale;
            }
            else
            {
                target.GetComponent<AI>().health -= damage;
                //audioData.Play();

            }

            yield return new WaitForSeconds(timeBetweenBurstShots);
        }
        Invoke("ResetAttack", reloadTime);
    }

    void ResetAttack()
    {
        ready = true;
    }
}
