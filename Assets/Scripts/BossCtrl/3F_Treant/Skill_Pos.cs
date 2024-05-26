using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Pos : MonoBehaviour
{
    [SerializeField]
    Transform Boss;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Boss.position;
        this.transform.rotation = Boss.rotation;
    }
}
