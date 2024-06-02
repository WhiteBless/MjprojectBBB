using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw_Skill_SFX : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.GMInstance.SoundManagerRef.Play_Assasin_SFX(SoundManager.Assasin_SFX.THROW_KNIFE);
    }
}
