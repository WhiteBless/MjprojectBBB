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
        this.transform.position = new Vector3(Player.position.x, Player.position.y, Player.position.z);
    }
}
