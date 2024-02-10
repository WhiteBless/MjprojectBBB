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

    public Ninja_CharacterController ninja_CharacterController;
    public InGameSetting inGameSetting;
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
                    if (getSkillTimes <= 0 && !ninja_CharacterController.isSkill2 && !ninja_CharacterController.isSkill3 && !ninja_CharacterController.isSkill4)
                    {
                        if (skillTimes > 0 && ninja_CharacterController.skill1)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
                        }
                    }
                    break;

                case SkillType.W:
                    if (getSkillTimes <= 0 && !ninja_CharacterController.isSkill1 && !ninja_CharacterController.isSkill3 && !ninja_CharacterController.isSkill4)
                    {
                        if (skillTimes > 0 && ninja_CharacterController.skill2)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
                        }
                    }
                    break;

                case SkillType.E:
                    if (getSkillTimes <= 0 && !ninja_CharacterController.isSkill1 && !ninja_CharacterController.isSkill2 && !ninja_CharacterController.isSkill4)
                    {
                        if (skillTimes > 0 && ninja_CharacterController.skill3)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
                        }
                    }
                    break;

                case SkillType.R:
                    if (getSkillTimes <= 0 && !ninja_CharacterController.isSkill1 && !ninja_CharacterController.isSkill2 && !ninja_CharacterController.isSkill3)
                    {
                        if (skillTimes > 0 && ninja_CharacterController.skill4)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
                        }
                    }
                    break;

                case SkillType.D:
                    if (getSkillTimes <= 0)
                    {
                        if (skillTimes > 0 && ninja_CharacterController.spaceDown)
                        {
                            hideSkillButtons.SetActive(true);
                            getSkillTimes = skillTimes;
                            isHideSkills = true;
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

        if (getSkillTimes > 0)
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
        }
    }
}