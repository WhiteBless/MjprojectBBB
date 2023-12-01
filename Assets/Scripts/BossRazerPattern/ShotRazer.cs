using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRazer : MonoBehaviour
{
    [Header("∑π¿Ã¿˙")]
    public Transform RazerPos_1;
    public Transform RazerPos_2;
    public Transform RazerPos_3;
    public Transform RazerPos_4;
    public Transform RazerPos_5;
    public Transform RazerPos_6;
    public GameObject Razer;

    public void UseRazer()
    {
        StartCoroutine("Shot");
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject intantRazer_1 = Instantiate(Razer, RazerPos_1.position, RazerPos_1.rotation);
        Rigidbody razerRigid_1 = intantRazer_1.GetComponent<Rigidbody>();
        razerRigid_1.velocity = RazerPos_1.forward * 50;

        GameObject intantRazer_2 = Instantiate(Razer, RazerPos_2.position, RazerPos_2.rotation);
        Rigidbody razerRigid_2 = intantRazer_2.GetComponent<Rigidbody>();
        razerRigid_2.velocity = RazerPos_2.forward * 50;

        GameObject intantRazer_3 = Instantiate(Razer, RazerPos_3.position, RazerPos_3.rotation);
        Rigidbody razerRigid_3 = intantRazer_3.GetComponent<Rigidbody>();
        razerRigid_3.velocity = RazerPos_3.forward * 50;

        GameObject intantRazer_4 = Instantiate(Razer, RazerPos_4.position, RazerPos_4.rotation);
        Rigidbody razerRigid_4 = intantRazer_4.GetComponent<Rigidbody>();
        razerRigid_4.velocity = RazerPos_4.forward * 50;

        GameObject intantRazer_5 = Instantiate(Razer, RazerPos_5.position, RazerPos_5.rotation);
        Rigidbody razerRigid_5 = intantRazer_5.GetComponent<Rigidbody>();
        razerRigid_5.velocity = RazerPos_5.forward * 50;

        GameObject intantRazer_6 = Instantiate(Razer, RazerPos_6.position, RazerPos_6.rotation);
        Rigidbody razerRigid_6 = intantRazer_6.GetComponent<Rigidbody>();
        razerRigid_6.velocity = RazerPos_6.forward * 50;
    }
}
