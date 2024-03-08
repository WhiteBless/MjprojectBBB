using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBall_Pilar : MonoBehaviour
{
    [SerializeField]
    Reaper_Controller reaper_ctrl;
    [SerializeField]
    float UpDownSpeed;

    // Update is called once per frame
    void Update()
    {
        if (reaper_ctrl.reaperState == ReaperState.Dark_Ball && this.transform.position.y < 0)
        {
            this.transform.Translate(Vector3.up * UpDownSpeed * Time.deltaTime);
        }
        else if (reaper_ctrl.reaperState != ReaperState.Dark_Ball && this.transform.position.y > -10)
        {
            this.transform.Translate(Vector3.down * UpDownSpeed * Time.deltaTime);
        }
    }
}
