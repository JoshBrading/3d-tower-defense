using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    AudioSource audioData;

    [Header("Peashooter")]
    public GameObject peashooter;
    public Vector3 peashooterRot;

    [Header("Repeater")]
    public GameObject repeater;
    public Vector3 repeaterRot;

    [Header("Sunflower")]
    public GameObject sunflower;
    public Vector3 sunflowerRot;

    private GameObject selected;

    public Material selectedMat, deselectedMat;
   // [Header("")]
   // public GameObject peashooter;
   // public GameObject peashooterRot;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log(hit.transform.tag);
                if( hit.transform.tag == "CanSpawn" && selected)
                {
                    int cost = selected.GetComponent<StoreInfo>().cost;
                    if( cost <= Commands.Instance.GetSuns())
                    {
                        Vector3 offset = new Vector3(0, -90, 0);
                        GameObject hitObj = hit.transform.gameObject;

                        GameObject plant = Commands.Instance.Spawn(selected, hitObj.transform.position, selected.transform.rotation.eulerAngles + offset, "Plant", "Ignore Raycast", hitObj); // FIX ME
                        plant.transform.localScale = new Vector3(1f, 1f, 1f);
                        plant.GetComponent<Renderer>().material = deselectedMat;

                        Commands.Instance.AddSuns(-cost);

                        audioData.Play();

                        selected.GetComponent<Renderer>().material = deselectedMat;
                        selected = null;
                        hit.transform.tag = "CannotSpawn";
                    }
                    
                }
                if(hit.transform.tag == "Selectable")
                {
                    if (selected) selected.GetComponent<Renderer>().material = deselectedMat;
                    selected = hit.transform.gameObject;
                    selected.GetComponent<Renderer>().material = selectedMat;
                }
            }
        }
    }
}
