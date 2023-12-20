using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    // 캐릭터 이동
    public abstract void Move();

    // 캐릭터 도약
    public abstract void Dodge();

    public abstract void DodgeOut();

    // 캐릭터 공격
    public abstract void Attack();

    // 캐릭터 스킬_1
    public abstract void Skill_1();
    // 캐릭터 스킬_2
    public abstract void Skill_2();
    // 캐릭터 스킬_3
    public abstract void Skill_3();
    // 캐릭터 스킬_4
    public abstract void Skill_4();

    // 입력값
    public abstract void GetInput();

    public abstract void SetMove(Vector3 move);

    public abstract void NotMove();

    public abstract void SkillOut();

    public abstract void AttackOut();

    public abstract void AAA_Attack();

    public abstract void ResetCollision();
}
