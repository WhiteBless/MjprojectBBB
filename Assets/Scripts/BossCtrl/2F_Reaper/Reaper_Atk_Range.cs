using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Atk_Range : MonoBehaviour
{
    public Transform obj;
    public Reaper_Controller reaperCtrl;

    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position + obj.forward * 5.0f;
    }
}
