using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Commands : MonoBehaviour
{
    public static Commands instance;

    public static Commands Instance
    {
        get { return instance ?? (instance = new GameObject("Commands").AddComponent<Commands>()); }
    }

    public GameObject Spawn(GameObject obj, Vector3 position, Vector3 rotation, string tag, string layer, GameObject parent )
    {
        GameObject s_obj = Instantiate(obj, position, Quaternion.Euler(rotation));

        if( tag != null ) s_obj.transform.tag = tag;
        if (layer != null ) s_obj.layer = LayerMask.NameToLayer(layer);
        if(parent) s_obj.transform.parent = parent.transform;

        Debug.Log("Spawned " + obj + " at position " + position);

        return s_obj;
    }

    public void AddSuns( int amount)
    {
        GameObject.Find("GenerateWorld").GetComponent<StoreManager>().AddSuns(amount);
    }

    public void SetSuns(int amount)
    {
        GameObject.Find("GenerateWorld").GetComponent<StoreManager>().SetSuns(amount);
    }

    public int GetSuns()
    {
        return GameObject.Find("GenerateWorld").GetComponent<StoreManager>().GetSuns();
    }
}
