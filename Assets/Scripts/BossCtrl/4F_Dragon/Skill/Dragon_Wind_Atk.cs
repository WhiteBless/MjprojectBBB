using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Wind_Atk : MonoBehaviour
{
    [SerializeField]
    float Wind_Speed;
    [SerializeField]
    float VisibleTime;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Wind_Speed * Time.deltaTime);
    }

    // 사라지게 하기 위한 코루틴
    private void OnEnable()
    {
        StartCoroutine(Visible_Time());
    }

    IEnumerator Visible_Time()
    {
        yield return new WaitForSeconds(VisibleTime);
        this.gameObject.SetActive(false);
    }
}
