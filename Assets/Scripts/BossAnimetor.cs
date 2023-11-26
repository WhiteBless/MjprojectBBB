using UnityEngine;

public class BossAnimator : MonoBehaviour
{

    public bool AttRadyState;

    // 보스의 상태를 나타내는 Enum
    public enum BossState
    {
        Idle  // 대기상태
    }

    public Animator animator;   // 애니메이터 컴포넌트

    private BossState currentState; // 현재 보스의 상태

    // 시작할 때 초기 상태를 설정합니다.
    private void Start()
    {
        currentState = BossState.Idle;
        animator.Play("Idle");
    }

    // 대기 상태로 변경하고 해당 상태에 맞는 애니메이션을 재생합니다.
    public void ChangeToIdleState()
    {
        if (currentState == BossState.Idle)
            return;

        currentState = BossState.Idle;

        // 애니메이터 컴포넌트의 애니메이션 클립을 설정하고 재생합니다.
        animator.Play("Idle");
    }
}