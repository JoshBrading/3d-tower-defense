using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SliderText : MonoBehaviour
{

    public void Increase()
    {
        StaticValues.rows++;
        this.GetComponent<TextMeshProUGUI>().text = StaticValues.rows.ToString();
        Debug.Log(StaticValues.rows);
    }
}

