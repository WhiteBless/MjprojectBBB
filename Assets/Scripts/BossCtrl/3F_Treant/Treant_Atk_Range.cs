using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treant_Atk_Range : MonoBehaviour
{
    public Transform obj;
    [SerializeField]
    Treant_Controller TreantCtrl;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = obj.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && TreantCtrl.isAttacking == false)
        {
            Debug.Log(2);
            // TreantCtrl.Treant_NextAct();
            // this.GetComponent<SphereCollider>().enabled = false;
        }
    }
}
