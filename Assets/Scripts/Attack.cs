using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //무기 타입, 데미지, 공속, 범위, 효과 변수 생성
    public enum Type { Melee, Range }

    [Header("무기 타입")]
    public Type type;

    [Header("무기 성능")]
    public int damage; //데미지
    public float rate; //공격속도
    public bool isAttackable;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
