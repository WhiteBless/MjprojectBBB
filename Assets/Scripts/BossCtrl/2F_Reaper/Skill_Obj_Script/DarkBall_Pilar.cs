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
        if (reaper_ctrl.reaper_NormalState == ReaperNormalState.Dark_Ball && this.transform.position.y < 0)
        {
            // 각성 전 기둥 활성화
            this.transform.Translate(Vector3.up * UpDownSpeed * Time.deltaTime);
        }
        else if(reaper_ctrl.reaper_AwakeState == ReaperAwakeState.Dark_Ball && this.transform.position.y < 0)
        {
            // 각성 기둥 활성화
            this.transform.Translate(Vector3.up * UpDownSpeed * Time.deltaTime);
        }
        else if (reaper_ctrl.reaper_NormalState != ReaperNormalState.Dark_Ball && this.transform.position.y > -10)
        {
            // 각성 전 기둥 비활성화
            this.transform.Translate(Vector3.down * UpDownSpeed * Time.deltaTime);
        }
        else if (reaper_ctrl.reaper_AwakeState == ReaperAwakeState.Dark_Ball && this.transform.position.y > -10)
        {
            // 각성 후 기둥 비활성화
            this.transform.Translate(Vector3.down * UpDownSpeed * Time.deltaTime);
        }
    }
}
