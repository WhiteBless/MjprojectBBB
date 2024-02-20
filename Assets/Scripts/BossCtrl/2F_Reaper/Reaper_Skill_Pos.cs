using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper_Skill_Pos : MonoBehaviour
{
    public Transform obj;

    [SerializeField]
    float Distance;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position + obj.forward * Distance;
    }
}
