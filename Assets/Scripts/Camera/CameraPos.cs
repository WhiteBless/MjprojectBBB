using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPos : MonoBehaviour
{
    [SerializeField]
    Transform Player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(Player.position.x, 0.0f, Player.position.z);
    }
}
