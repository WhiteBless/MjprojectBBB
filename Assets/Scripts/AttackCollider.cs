using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    [Header("근접 공격 범위")]
    public BoxCollider swordArea;

    [Header("원거리 공격 범위")]
    public BoxCollider standoffArea_1;
    public BoxCollider standoffArea_2;
    public BoxCollider standoffArea_3;
    public BoxCollider standoffArea_4;
    public BoxCollider standoffArea_5;
    public BoxCollider standoffArea_6;
    public BoxCollider standoffArea_7;

    [Header("범위 공격 범위")]
    public BoxCollider RnageArea;

    [Header("공격 이펙트")]
    public SkinnedMeshRenderer FX_Slash_R;
    public SkinnedMeshRenderer R_Shash_FX;
    public SkinnedMeshRenderer R_Shash_FX001;
    public SkinnedMeshRenderer Treak_Weapon;

    public void A_Use()
    {
        StartCoroutine("AAA");
        StartCoroutine("AAAAA");
    }

    public void A_NoUse()
    {
        StopCoroutine("AAA");
        StopCoroutine("AAAAA");
    }

    public void Q_Use()
    {
        StartCoroutine("QQQ"); 
    }

    public void W_Use()
    {
        StartCoroutine("WWW"); 
    }

    public void E_Use()
    {
        StartCoroutine("EEE");

    }

    public void R_Use()
    {
        StartCoroutine("RRR"); 
    }

    IEnumerator AAA()
    {
        swordArea.enabled = true;

        standoffArea_1.enabled = true;
        standoffArea_2.enabled = true;
        standoffArea_3.enabled = true;
        standoffArea_4.enabled = true;
        standoffArea_5.enabled = true;
        standoffArea_6.enabled = true;
        standoffArea_7.enabled = true;

        /*Treak_Weapon.enabled = true;

        FX_Slash_R.enabled = true;
        R_Shash_FX.enabled = true;
        R_Shash_FX001.enabled = true;*/

        yield return new WaitForSeconds(0.45f);
        swordArea.enabled = false;

        standoffArea_1.enabled = false;
        standoffArea_2.enabled = false;
        standoffArea_3.enabled = false;
        standoffArea_4.enabled = false;
        standoffArea_5.enabled = false;
        standoffArea_6.enabled = false;
        standoffArea_7.enabled = false;

        /*Treak_Weapon.enabled = false;

        FX_Slash_R.enabled = false;
        R_Shash_FX.enabled = false;
        R_Shash_FX001.enabled = false;*/
    }

    IEnumerator AAAAA()
    {
        yield return new WaitForSeconds(0.12f);
        Treak_Weapon.enabled = true;

        FX_Slash_R.enabled = true;
        R_Shash_FX.enabled = true;
        R_Shash_FX001.enabled = true;

        yield return new WaitForSeconds(0.45f);
        Treak_Weapon.enabled = false;

        FX_Slash_R.enabled = false;
        R_Shash_FX.enabled = false;
        R_Shash_FX001.enabled = false;
    }


    IEnumerator QQQ()
    {
        yield return new WaitForSeconds(0.001f);
        standoffArea_1.enabled = true;
        standoffArea_2.enabled = true;
        standoffArea_3.enabled = true;
        standoffArea_4.enabled = true;
        standoffArea_5.enabled = true;
        standoffArea_6.enabled = true;
        standoffArea_7.enabled = true;

        Treak_Weapon.enabled = true;

        yield return new WaitForSeconds(1.3f);
        standoffArea_1.enabled = false;
        standoffArea_2.enabled = false;
        standoffArea_3.enabled = false;
        standoffArea_4.enabled = false;
        standoffArea_5.enabled = false;
        standoffArea_6.enabled = false;
        standoffArea_7.enabled = false;

        Treak_Weapon.enabled = false;
    }

    IEnumerator WWW()
    {
        yield return new WaitForSeconds(0.001f);
        swordArea.enabled = true;

        FX_Slash_R.enabled = true;
        R_Shash_FX.enabled = true;

        standoffArea_1.enabled = true;
        standoffArea_2.enabled = true;
        standoffArea_3.enabled = true;
        standoffArea_4.enabled = true;
        standoffArea_5.enabled = true;
        standoffArea_6.enabled = true;
        standoffArea_7.enabled = true;

        Treak_Weapon.enabled = true;

        yield return new WaitForSeconds(1.5f);
        swordArea.enabled = false;

        standoffArea_1.enabled = false;
        standoffArea_2.enabled = false;
        standoffArea_3.enabled = false;
        standoffArea_4.enabled = false;
        standoffArea_5.enabled = false;
        standoffArea_6.enabled = false;
        standoffArea_7.enabled = false;

        Treak_Weapon.enabled = false;

        FX_Slash_R.enabled = false;
        R_Shash_FX.enabled = false;
    }

    IEnumerator EEE()
    {
        yield return new WaitForSeconds(0.1f);
        swordArea.enabled = true;

        Treak_Weapon.enabled = true;

        yield return new WaitForSeconds(0.6f);
        swordArea.enabled = false;

        Treak_Weapon.enabled = false;
    }

    IEnumerator RRR()
    {
        yield return new WaitForSeconds(0.1f);
        RnageArea.enabled = true;

        Treak_Weapon.enabled = true;

        FX_Slash_R.enabled = true;

        yield return new WaitForSeconds(1f);
        RnageArea.enabled = false;

        Treak_Weapon.enabled = false;

        FX_Slash_R.enabled = false;
    }
}
