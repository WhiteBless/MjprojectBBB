using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Active_Time : MonoBehaviour
{
    [SerializeField]
    float ActiveTime;

    void OnEnable()
    {
        StartCoroutine(Active_Time());
    }

    IEnumerator Active_Time()
    {
        yield return new WaitForSeconds(ActiveTime);
        this.transform.gameObject.SetActive(false);
    }
}
