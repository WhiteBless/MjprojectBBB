using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordParticle_Eff : MonoBehaviour
{
    public ParticleSystem[] childParticleSystems;
    public ParticleSystem particleSystem;
    public bool isBig;
    public bool isShot;

    [SerializeField]
    float UpScale;

    private Vector3 StartScale;
    float timer;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
            StartScale = transform.localScale;
        }

        if (particleSystem == null)
        {
            Debug.LogError("Particle system is not assigned or found.");
        }
    }

    void Update()
    {
        // 특정 조건을 만족할 때 파티클 시스템을 멈춤
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(stopparticle());   
        }

        if (isBig == true)
        {
            // 현재 크기에서 점점 커지게 하기
            float scaleFactor = 1.0f + UpScale * Time.deltaTime;

            // 크기 변경
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scaleFactor, scaleFactor, scaleFactor));
            //this.transform.localScale = new Vector3(transform.localScale.x * UpScale * Time.deltaTime,
            //    transform.localScale.y * UpScale * Time.deltaTime,
            //    transform.localScale.z * UpScale * Time.deltaTime);
        }

        if (isShot == true)
        {
            // 로컬 좌표 기준으로 이동
            transform.Translate(Vector3.forward * 100.0f * Time.deltaTime, Space.Self);
        }
    }

    IEnumerator stopparticle()
    {
        // yield return new WaitForSeconds(0.06f);
        //yield return new WaitForSeconds(10.0f);
        //StopParticleSystem();

        // 자식 파티클 시스템들을 일시 중지 또는 재생
        //foreach (ParticleSystem childParticleSystem in childParticleSystems)
        //{
        //    if (childParticleSystem.isPlaying)
        //    {
        //        childParticleSystem.Pause();
        //    }
        //    else
        //    {
        //        childParticleSystem.Play();
        //    }
        //}

        yield return new WaitForSeconds(5.0f);
        this.gameObject.transform.localScale = StartScale;
        isShot = false;
        this.gameObject.SetActive(false);
    }

    void StopParticleSystem()
    {
        particleSystem.Pause();
    }
}
