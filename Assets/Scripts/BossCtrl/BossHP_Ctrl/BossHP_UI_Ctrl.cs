using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP_UI_Ctrl : MonoBehaviour
{
    [Header("--HP_Image--")]
    [SerializeField]
    Image CurBoss_HP_Img;
    [SerializeField]
    Image NextBoss_HP_Img;
    [SerializeField]
    Image TakeDamage_Img;

    [Tooltip("One Line Hp Value")]
    [Header("--HP_Var--")]
    [SerializeField]
    int Boss_SingleBarHP;

    [Tooltip("Hp Value")]
    public int BossMax_HP;
    public int BossCur_HP;
    public int Before_Boss_HP;

    [SerializeField]
    int remain_BossHP_Line;

    [Tooltip("Hp Colors")]
    public List<Color> HP_Colors;

    public Text HP_Text;

    // Start is called before the first frame update
    void Start()
    {
        // Refresh_BossHP();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Refresh_BossHP()
    {
        // 사이즈 비율 조정
        CurBoss_HP_Img.rectTransform.sizeDelta =
            new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
            NextBoss_HP_Img.rectTransform.sizeDelta.y);

        // 현재 체력 색 조정
        CurBoss_HP_Img.color = GetColorByLayer(BossCur_HP);
        StartCoroutine(TakeDamageBar_Refresh());

        // 다음 체력바 색으로 표시, 0이하로 떨어질 시 검정색으로 표시
        NextBoss_HP_Img.color = GetColorByLayer(BossCur_HP - Boss_SingleBarHP);

        HP_Text.text = "X " + (int)(BossCur_HP / Boss_SingleBarHP);

        if (remain_BossHP_Line != (int)(BossCur_HP / Boss_SingleBarHP))
        {
            // 사이즈 비율 조정
            TakeDamage_Img.rectTransform.sizeDelta =
                new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
                NextBoss_HP_Img.rectTransform.sizeDelta.y);
        }

        remain_BossHP_Line = (int)(BossCur_HP / Boss_SingleBarHP);
    }

    IEnumerator TakeDamageBar_Refresh()
    {
        yield return new WaitForSeconds(1.0f);

        // 사이즈 비율 조정
        TakeDamage_Img.rectTransform.sizeDelta =
            new Vector2(NextBoss_HP_Img.rectTransform.sizeDelta.x * GetHPRatioSingleBar(BossCur_HP),
            NextBoss_HP_Img.rectTransform.sizeDelta.y);
    }

    float GetHPRatioSingleBar(int _targetHP)
    {
        float result_Ratio = 0.0f;

        // 보스 HP가 0보다 클 때
        if (_targetHP > 0)
        {
            float divided = (float)_targetHP / Boss_SingleBarHP;

            // 맨끝에 있을 때 0 or 1
            if (divided == (int)divided)
            {
                result_Ratio = 1;
            }
            else
            {
                float moduled = _targetHP % Boss_SingleBarHP;

                result_Ratio = moduled / Boss_SingleBarHP;
            }
        }
        else
        {
            result_Ratio = 0.0f;
        }

        return result_Ratio;
    }

    Color GetColorByLayer(int _targetHP)
    {
        // 기본 컬러는 검정색
        Color result = Color.black;

        // 보스의 체력이 남아있을 경우
        if (_targetHP > 0)
        {

            // 현재 체력에서 한줄 체력을 나누기 위한 
            float divided = (float)_targetHP / Boss_SingleBarHP;

            // 나눈 부분을 인덱스로 가져옴
            int index = (int)divided;

            if (divided == (int)divided)
            {
                index--;
            }

            result = HP_Colors[index % HP_Colors.Count];
        }

        return result;
    }
}
