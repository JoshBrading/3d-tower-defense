using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThink : MonoBehaviour
{
    public Vector3 angularVelocity = new Vector3(0,0,0);
    public float damage;
    public float lifetime;
    //public LayerMask layermask;
    AudioSource audioData;
    public AudioClip hit;


    void Start()
    {
        audioData = GetComponent<AudioSource>();

        StartCoroutine(DieAfterLifetime( lifetime ));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.GetComponent<Rigidbody>().velocity = angularVelocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<AI>().health -= damage;
        //Debug.Log(collision.gameObject.GetComponent<AI>().health);
        audioData.PlayOneShot(hit, 1);
        Die();
    }

    void Die()
    {
        Object.Destroy(this.gameObject);
    }

    IEnumerator DieAfterLifetime( float lifetime )
    {
        yield return new WaitForSeconds(lifetime);
        Die();
    }
}
