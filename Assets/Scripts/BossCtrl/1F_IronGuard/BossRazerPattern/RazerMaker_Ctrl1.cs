using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RazerMaker_Ctrl1 : MonoBehaviour
{
    public bool isRazerAtk;

    // Update is called once per frame
    void Update()
    {
        // 레이저 발사 오브젝트가 올라오도록 조정
        if (this.transform.position.y < 0.0f && isRazerAtk == false)
        {
            this.transform.Translate(Vector3.up * 10.0f * Time.deltaTime, Space.World);
        }

        // 레이저 발사 후 오브젝트가 내려가도록 조정
        if (isRazerAtk == true && this.transform.position.y > -10.0f)
        {
            this.transform.Translate(Vector3.down * 10.0f * Time.deltaTime, Space.World);
        }
    }
}
