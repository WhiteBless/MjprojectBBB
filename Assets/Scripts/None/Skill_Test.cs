using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Skill_Test : MonoBehaviour
{
    public enum SkillType { Q, W, E, R, D };

    [Header("스킬 유형")]
    public SkillType skillType;

    [Header("쿨타임")]
    public GameObject hideSkillButtons;
    public GameObject textPros;
    public TextMeshProUGUI hideSkillTimeTexts;
    public Image hideSkillmages;
    private bool isHideSkills = false;
    public float skillTimes = 30;
    public float getSkillTimes = 0;

    public bool isSkill1CT;
    public bool isSkill2CT;
    public bool isSkill3CT;
    public bool isSkill4CT;
    public bool isDodgeCT;

    public InGameSetting inGameSetting;

    public Assassin_Controller assassin_Controller;
    public Samurai_Controller samurai_Controller;


    void Init()
    {
        // TODO ## 캐릭터 스킬 UI 쪽 캐릭터 스폰 시 정보 들고 오는곳
        // 현재 캐릭터가 어쌔신이면
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            assassin_Controller = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>();
        }
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
        {
            samurai_Controller = GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Samurai_Controller>();
        }
    }


    void Start()
    {
        hideSkillTimeTexts = textPros.GetComponent<TextMeshProUGUI>();
        hideSkillButtons.SetActive(false);

        Init();
    }

    void Update()
    {
        HideSkillChk();

        HideSkillSetting();
    }

    public void HideSkillSetting()
    {
        if (!inGameSetting.isPaused)
        {
            switch (skillType)
            {
                case SkillType.Q:
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
                    {
                        if (!isSkill1CT && !assassin_Controller.isSkill2 && !assassin_Controller.isSkill3 && !assassin_Controller.isSkill4 && !assassin_Controller.isDodge && !assassin_Controller.isAttack)
                        {
                            if (skillTimes > 0f && assassin_Controller.skill1)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill1CT = true;
                            }
                        }
                    }
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
                    {
                        if (!isSkill1CT && !samurai_Controller.isSkill2 && !samurai_Controller.isSkill3 && !samurai_Controller.isSkill4 && !samurai_Controller.isDodge && !samurai_Controller.isAttack)
                        {
                            if (skillTimes > 0f && samurai_Controller.skill1)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill1CT = true;
                            }
                        }
                    }

                        break;

                case SkillType.W:
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
                    {
                        if (!isSkill2CT && !assassin_Controller.isSkill1 && !assassin_Controller.isSkill3 && !assassin_Controller.isSkill4 && !assassin_Controller.isDodge && !assassin_Controller.isAttack)
                        {
                            if (skillTimes > 0f && assassin_Controller.skill2)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill2CT = true;
                            }
                        }
                    }
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
                    {
                        if (!isSkill2CT && !samurai_Controller.isSkill1 && !samurai_Controller.isSkill3 && !samurai_Controller.isSkill4 && !samurai_Controller.isDodge && !samurai_Controller.isAttack)
                        {
                            if (samurai_Controller.isComboTimeout)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill2CT = true;
                            }
                        }
                    }
                    break;

                case SkillType.E:
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
                    {
                        if (!isSkill3CT && !assassin_Controller.isSkill1 && !assassin_Controller.isSkill2 && !assassin_Controller.isSkill4 && !assassin_Controller.isDodge && !assassin_Controller.isAttack)
                        {
                            if (skillTimes > 0f && assassin_Controller.skill3)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill3CT = true;
                            }
                        }
                    }
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
                    {
                        if (!isSkill3CT && !samurai_Controller.isSkill1 && !samurai_Controller.isSkill2 && !samurai_Controller.isSkill4 && !samurai_Controller.isDodge && !samurai_Controller.isAttack)
                        {
                            if (skillTimes > 0f && samurai_Controller.skill3)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill3CT = true;
                            }
                        }
                    }
                        break;

                case SkillType.R:
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
                    {
                        if (!isSkill4CT && !assassin_Controller.isSkill1 && !assassin_Controller.isSkill2 && !assassin_Controller.isSkill3 && !assassin_Controller.isDodge && !assassin_Controller.isAttack)
                        {
                            if (skillTimes > 0f && assassin_Controller.skill4)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill4CT = true;
                            }
                        }
                    }
                    if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
                    {
                        if (!isSkill4CT && !samurai_Controller.isSkill1 && !samurai_Controller.isSkill2 && !samurai_Controller.isSkill3 && !samurai_Controller.isDodge && !samurai_Controller.isAttack)
                        {
                            if (skillTimes > 0f && samurai_Controller.skill4)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isSkill4CT = true;
                            }
                        }
                    }
                        break;

                case SkillType.D:
                    if (getSkillTimes <= 0f)
                    {
                        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
                        {
                            // TODO ## 어쌔신 클래스 전용 닷지 스킬
                            if (!isDodgeCT && assassin_Controller.spaceDown && assassin_Controller.isHitOut)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isDodgeCT = true;
                            }
                        }
                        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
                        {
                            if (!isDodgeCT && samurai_Controller.spaceDown && samurai_Controller.isHitOut)
                            {
                                hideSkillButtons.SetActive(true);
                                getSkillTimes = skillTimes;
                                isHideSkills = true;
                                isDodgeCT = true;
                            }
                        }
                    }
                    break;
            }
        }
    }

    private void HideSkillChk()
    {
            if (isHideSkills)
            {
                StartCoroutine(SkillTimeCHk());
            }
    }

    IEnumerator SkillTimeCHk()
    {
        yield return null;

        if (getSkillTimes > 0f)
        {
            getSkillTimes -= Time.deltaTime;

            hideSkillTimeTexts.text = getSkillTimes.ToString("00");

            float time = getSkillTimes / skillTimes;
            hideSkillmages.fillAmount = time;
        }

        else
        {
            isHideSkills = false;
            hideSkillButtons.SetActive(false);
            ResetCT();
        }
    }

    private void ResetCT()
    {
        isDodgeCT = false;
        isSkill1CT = false;
        isSkill2CT = false;
        isSkill3CT = false;
        isSkill4CT = false;
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.SAMURAI)
        {
            samurai_Controller.isComboTimeout = false;
        }  
    }
}