using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AI : MonoBehaviour
{
    AudioSource audioData;

    public AudioClip sunGenSFX;
    public AudioClip damagedSFX;


    [Header("Basic")]
    public float health;
    private float oldHealth;
    public LayerMask targetLayer;

    [Header("Sun Generation")]
    public GameObject floatingPoints;
    public int sunGen;
    public float sunDelay, sunRepeat;

    [Header("Movement")]
    public bool canMove;
    [Range(0f, 10f)]
    public float movementSpeed = 2;
    public float attackPauseDuration;

    [Header("Advanced")]
    public int rayCount = 1;
    public Vector3 rayOffset;

    public float viewDistance;

    void Start()
    {
        if (sunGen != 0 && gameObject.layer != 28) InvokeRepeating("SunGenerator", sunDelay, sunRepeat);
        movementSpeed = movementSpeed / -100;
        //viewDistance = GameObject.Find("GenerateWorld").GetComponent<GenerateWorld>().col;

        if(sunGen != 0 && gameObject.layer != 28)
        {
            audioData = GetComponent<AudioSource>();
        }

        oldHealth = health;
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + rayOffset, transform.TransformDirection(Vector3.forward * -1), out hit, viewDistance, targetLayer))
        {
            Debug.DrawRay(transform.position + rayOffset, transform.TransformDirection(Vector3.forward * -1) * hit.distance, Color.red);
            this.GetComponent<WeaponSystem>().TriggerAttack( hit.transform.gameObject );
            if( canMove )
            {
                canMove = false;
                this.GetComponent<Animator>().SetBool("IsAttacking", true);

            }
        }
        else
        {
            Debug.DrawRay(transform.position + rayOffset, transform.TransformDirection(Vector3.forward * -1) * viewDistance, Color.white);
            if (this.GetComponent<Animator>())
            {
                this.GetComponent<Animator>().SetBool("IsAttacking", false);
                canMove = true;
            }
        }

        if (canMove)
        {
            transform.position = transform.position + new Vector3(movementSpeed, 0, 0);
        }

        if (health <= 0) Die();
        if (health != oldHealth)
        {
            audioData.PlayOneShot(damagedSFX, 1);
            oldHealth = health;
        }
        if ( transform.position.x < -1)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }

    void ResetMove()
    {
        canMove = true;      
    }

    void SunGenerator()
    {
        Commands.Instance.AddSuns(sunGen);
        audioData.PlayOneShot( sunGenSFX, 1);

        GameObject sunText = Instantiate(floatingPoints, transform.position, Quaternion.identity) as GameObject;
        sunText.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + sunGen.ToString();
        StartCoroutine(KillObjectAfterSeconds(sunText, 1));

    }

    IEnumerator KillObjectAfterSeconds( GameObject go, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Object.Destroy(go.gameObject);
    }

    void Die()
    {
        Object.Destroy(this.gameObject);

        if( this.transform.tag == "Plant")
        {
            this.transform.parent.gameObject.transform.tag = "CanSpawn";
        }
    }
}