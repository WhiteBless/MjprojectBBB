using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Active : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Wall"))
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && other.gameObject.GetComponent<MeshRenderer>().enabled == false)
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
