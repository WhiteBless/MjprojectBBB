using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour
{
    public Animator animator;   // 애니메이터 컴포넌트
    public bool AttRadyState;   // AttRadyState 변수

    private void Start()
    {
        // 일정 시간 뒤에 상태 변경을 위한 코루틴 실행
        StartCoroutine(ChangeStateAfterDelay(5f));
    }


    IEnumerator ChangeStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 상태 변경 코드 작성
        animator.SetBool("doLookPlayer", AttRadyState);
    }

    // 애니메이션을 처음 상태로 되돌립니다.
    public void ResetAnimation()
    {
        // 애니메이션 파라미터 초기화
        animator.SetBool("doLookPlayer", false);

    }

    // 'doLookPlayer' 애니메이션을 처음부터 시작합니다.
    public void StartAnimation()
    {
        // 애니메이션 파라미터 설정
        animator.SetBool("doLookPlayer", true);

        // 'doLookPlayer' 애니메이션 클립을 처음부터 재생
        animator.Play("doLookPlayer", -1, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // AttRadyState 값에 따라 애니메이션 상태를 변경합니다.
        if (AttRadyState)
        {
            StartAnimation();
        }
        else
        {
            ResetAnimation();
        }
    }
}