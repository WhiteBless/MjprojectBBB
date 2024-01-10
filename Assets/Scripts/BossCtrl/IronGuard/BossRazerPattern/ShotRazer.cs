using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRazer : MonoBehaviour
{
    [Header("레이저")]
    public GameObject[] Beams;
    public GameObject[] Electrics;

    public Transform RazerPos_1;
    public Transform RazerPos_2;
    public Transform RazerPos_3;
    public Transform RazerPos_4;
    public Transform RazerPos_5;
    public Transform RazerPos_6;

    [SerializeField]
    GameObject Razer;
    [SerializeField]
    float Beam_Speed;

    [SerializeField]
    IronGuard_ObjPool Razer_Pooling;

    public bool isShot;

    void Update()
    {
        // 레이저 공격 실행시키면
        if(isShot == true)
        {
            for (int i = 0; i < Beams.Length; i++)
            {
                Electrics[i].SetActive(true);
                // 레이저 늘리기
                float newYScale = Beams[i].transform.localScale.y + Beam_Speed * Time.deltaTime;
                Beams[i].transform.localScale = new Vector3(Beams[i].transform.localScale.x, newYScale, Beams[i].transform.localScale.z);

                // Beams[i].transform.localScale = new Vector3(0.1f, Beam_Speed * Time.deltaTime, 0.1f);
            }
        }
    }

    public void End_Razer_Atk()
    {
        isShot = false;

        for (int i = 0; i < Beams.Length; i++)
        {
            Beams[i].transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Electrics[i].SetActive(false);
        }
    }


    public void UseRazer()
    {
        StartCoroutine("Shot");
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.5f);
        //GameObject intantRazer_1 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_1.transform.position = RazerPos_1.position;
        //intantRazer_1.transform.rotation = RazerPos_1.rotation;
        GameObject intantRazer_1 = Instantiate(Razer, RazerPos_1.position, RazerPos_1.rotation);
        Rigidbody razerRigid_1 = intantRazer_1.GetComponent<Rigidbody>();
        razerRigid_1.velocity = RazerPos_1.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_1));  

        //GameObject intantRazer_2 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_2.transform.position = RazerPos_2.position;
        //intantRazer_2.transform.rotation = RazerPos_2.rotation;
        GameObject intantRazer_2 = Instantiate(Razer, RazerPos_2.position, RazerPos_2.rotation);
        Rigidbody razerRigid_2 = intantRazer_2.GetComponent<Rigidbody>();
        razerRigid_2.velocity = RazerPos_2.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_2));

        //GameObject intantRazer_3 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_3.transform.position = RazerPos_3.position;
        //intantRazer_3.transform.rotation = RazerPos_3.rotation;
        GameObject intantRazer_3 = Instantiate(Razer, RazerPos_3.position, RazerPos_3.rotation);
        Rigidbody razerRigid_3 = intantRazer_3.GetComponent<Rigidbody>();
        razerRigid_3.velocity = RazerPos_3.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_3));

        //GameObject intantRazer_4 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_4.transform.position = RazerPos_4.position;
        //intantRazer_4.transform.rotation = RazerPos_4.rotation;
        GameObject intantRazer_4 = Instantiate(Razer, RazerPos_4.position, RazerPos_4.rotation);
        Rigidbody razerRigid_4 = intantRazer_4.GetComponent<Rigidbody>();
        razerRigid_4.velocity = RazerPos_4.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_4));

        //GameObject intantRazer_5 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_5.transform.position = RazerPos_5.position;
        //intantRazer_5.transform.rotation = RazerPos_5.rotation;
        GameObject intantRazer_5 = Instantiate(Razer, RazerPos_5.position, RazerPos_5.rotation);
        Rigidbody razerRigid_5 = intantRazer_5.GetComponent<Rigidbody>();
        razerRigid_5.velocity = RazerPos_5.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_5));

        //GameObject intantRazer_6 = Razer_Pooling.Razer_GetObjectFromPool();
        //intantRazer_6.transform.position = RazerPos_6.position;
        //intantRazer_6.transform.rotation = RazerPos_6.rotation;
        GameObject intantRazer_6 = Instantiate(Razer, RazerPos_6.position, RazerPos_6.rotation);
        Rigidbody razerRigid_6 = intantRazer_6.GetComponent<Rigidbody>();
        razerRigid_6.velocity = RazerPos_6.forward * 50;
        StartCoroutine(Razer_ShowF(intantRazer_6));
    }


    IEnumerator Razer_ShowF(GameObject razer)
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(razer);
    }
}
