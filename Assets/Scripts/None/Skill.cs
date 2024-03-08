using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Skill : MonoBehaviour
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

    public Player player;

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
        switch (skillType)
        {
            case SkillType.Q:
                if (getSkillTimes <= 0 && !player.isSkill2 && !player.isSkill3 && !player.isSkill4)
                {
                    if (skillTimes > 0 && player.skill1)
                    {
                        hideSkillButtons.SetActive(true);
                        getSkillTimes = skillTimes;
                        isHideSkills = true;
                    }
                }     
                break;

            case SkillType.W:
                if (getSkillTimes <= 0 && !player.isSkill1 && !player.isSkill3 && !player.isSkill4)
                {
                    if (skillTimes > 0 && player.skill2)
                    {
                        hideSkillButtons.SetActive(true);
                        getSkillTimes = skillTimes;
                        isHideSkills = true;
                    }
                }
                break;

            case SkillType.E:
                if (getSkillTimes <= 0 && !player.isSkill1 && !player.isSkill2 && !player.isSkill4)
                {
                    if (skillTimes > 0 && player.skill3)
                    {
                        hideSkillButtons.SetActive(true);
                        getSkillTimes = skillTimes;
                        isHideSkills = true;
                    }
                }   
                break;

            case SkillType.R:
                if (getSkillTimes <= 0 && !player.isSkill1 && !player.isSkill2 && !player.isSkill3)
                {
                    if (skillTimes > 0 && player.skill4)
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
                    if (skillTimes > 0 && player.spaceDown)
                    {
                        hideSkillButtons.SetActive(true);
                        getSkillTimes = skillTimes;
                        isHideSkills = true;
                    }
                }    
                break;
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