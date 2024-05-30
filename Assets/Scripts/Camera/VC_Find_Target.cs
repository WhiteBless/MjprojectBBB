using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VC_Find_Target : MonoBehaviour
{
    [SerializeField]
    Transform Target;

    void Start()
    {
        Target = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(1);
    }

    void Update()
    {
        this.transform.rotation = Target.rotation;
        this.transform.position = Target.position;
    }
}
