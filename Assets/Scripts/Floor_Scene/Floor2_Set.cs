using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor2_Set : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GMInstance.cur_Scene = Define.Cur_Scene.FLOOR_2;
    }
}
