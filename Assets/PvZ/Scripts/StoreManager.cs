using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{

    [Header("Setup")]
    public GameObject floatingPoints;
    public GameObject planterBox, SolarSystemGroup, textBox;
    public Vector3 textOffset;
    public Vector3 textRot;
    public int sunGenTimer, sunGenDelay, sunsPerCollect;

    public GameObject[] plants;

    public int suns;
    private GameObject solar;
    private GameObject sunCounter;

    void Start()
    {
        InvokeRepeating("SunGenerator", sunGenDelay, sunGenTimer);
        GenerateStore();
    }

    // Update is called once per frame
    void Update()
    {
        if( sunCounter ) sunCounter.GetComponent<TextMeshPro>().text = suns + "\nSUNS";
    }

    void SunGenerator()
    {
        suns += sunsPerCollect;
        GameObject sunText = Instantiate(floatingPoints, new Vector3(0, 0, -2), Quaternion.identity) as GameObject;
        sunText.transform.GetChild(0).GetComponent<TextMesh>().text = "+" + sunsPerCollect.ToString();
        StartCoroutine(KillObjectAfterSeconds(sunText, 1));

    }

    IEnumerator KillObjectAfterSeconds(GameObject go, int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Object.Destroy(go.gameObject);
    }

    void GenerateStore()
    {
        Vector3 spawnPos;
        Vector3 rotation = new Vector3(0, 0, 0);
        int numPlants = plants.Length;

        solar = Commands.Instance.Spawn(SolarSystemGroup, new Vector3(0, 0, -2), rotation, null, null, null);
        sunCounter = GameObject.Find("SunCount");

        for (int col = 0; col < numPlants; col++)
        {

            spawnPos = new Vector3(col + 2, 0, -2);
            GameObject store = Commands.Instance.Spawn(planterBox, spawnPos, rotation, null, null, null);
            GameObject plant = Commands.Instance.Spawn(plants[col], spawnPos, plants[col].transform.rotation.eulerAngles, "Selectable", "StoreItem", store);
            GameObject storeText = Commands.Instance.Spawn(textBox, spawnPos + textOffset, textRot, null, null, null);

            string name = plant.GetComponent<StoreInfo>().storeName;
            int cost = plant.GetComponent<StoreInfo>().cost;

            storeText.GetComponent<TextMeshPro>().text = name + "\n" + cost;
        }
    }

    public void AddSuns( int amount )
    {
        suns += amount;
    }

    public void SetSuns(int amount)
    {
        suns = amount;
    }

    public int GetSuns()
    {
        return suns;
    }
}
