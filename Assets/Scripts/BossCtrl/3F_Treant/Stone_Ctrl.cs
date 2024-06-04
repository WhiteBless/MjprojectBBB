using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Ctrl : MonoBehaviour
{
    [SerializeField]
    Transform StoneStart_Parent;
    [SerializeField]
    float UpSpeed;
    [SerializeField]
    float ThrowSpeed;
    [SerializeField]
    bool isActiveT;
    public bool isSeize;
    public bool isThrow;

    public float Forward_Size;

    [SerializeField]
    Transform Explosion_Obj;

    void OnEnable()
    {
        GameManager.GMInstance.SoundManagerRef.Play_3FBoss_SFX(SoundManager.Boss_3F_SFX.SPAWNSTONE);
        Quaternion rotation = Quaternion.identity;
        this.transform.rotation = rotation;
        // 처음은 던지지 않으니
        isThrow = false;
        // 잡히지 않았으니 false
        isSeize = false;
        // active 활성화
        isActiveT = true;
        // 시작 부모 오브젝트 초기화
        this.transform.parent = StoneStart_Parent;
    }

    // Update is called once per frame
    void Update()
    {
        // 위에 올라오는 로직
        if (isActiveT && this.transform.localPosition.y <= 10.0f && !isSeize)
        {
            this.transform.Translate(Vector3.up * UpSpeed * Time.deltaTime);
        }
        
        if (isThrow == true)
        {
            this.transform.Translate(Vector3.forward * ThrowSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground") && isThrow)
        {
            GameManager.GMInstance.SoundManagerRef.Play_3FBoss_SFX(SoundManager.Boss_3F_SFX.GROUND_EXPLOSIN);
            // 폭발 이펙트 생성
            Explosion_Obj.GetChild(0).gameObject.SetActive(true);
            Explosion_Obj.GetChild(0).gameObject.transform.position = this.transform.position;

            this.gameObject.SetActive(false);
        }
    }
}
