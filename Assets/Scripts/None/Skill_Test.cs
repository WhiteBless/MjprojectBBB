using System.Collections;
using System.Collections.Generic;
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

    public Ninja_CharacterController ninja_CharacterController;
    public InGameSetting inGameSetting;

    public Assassin_Controller assassin_Controller;
    void Start()
    {
        hideSkillTimeTexts = textPros.GetComponent<TextMeshProUGUI>();
        hideSkillButtons.SetActive(false);
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
                    break;

                case SkillType.W:
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
                    break;

                case SkillType.E:
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
                    break;

                case SkillType.R:
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
                    break;

                case SkillType.D:
                    if (getSkillTimes <= 0f)
                    {
                        if (!isDodgeCT && assassin_Controller.spaceDown)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
                            isDodgeCT = true;
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
    }
}