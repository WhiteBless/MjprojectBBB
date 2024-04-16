using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Atk_Range : MonoBehaviour
{
    public Transform obj;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position;
        this.transform.rotation = obj.rotation;
    }
}
