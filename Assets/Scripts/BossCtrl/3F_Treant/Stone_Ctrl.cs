using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone_Ctrl : MonoBehaviour
{
    [SerializeField]
    Transform StartPos;
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


    void OnEnable()
    {

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
        // 위치 초기화
        Vector3 Pos = StartPos.position + StartPos.forward * 15.0f;
        this.transform.position = new Vector3(Pos.x, -4.0f, Pos.z);
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
            this.gameObject.SetActive(false);
        }
    }
}
