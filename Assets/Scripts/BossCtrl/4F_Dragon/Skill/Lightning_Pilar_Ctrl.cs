using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning_Pilar_Ctrl : MonoBehaviour
{
    public bool isUp;
    public float Pilar_Speed;

    private void OnEnable()
    {
        isUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isUp && this.transform.localPosition.z >= 0)
        {
            this.transform.Translate(Vector3.back * Pilar_Speed * Time.deltaTime);
        }
        else if(!isUp && this.transform.localPosition.z >= 0)
        {
            this.transform.Translate(Vector3.back * -Pilar_Speed * Time.deltaTime);
        }
    }
}
