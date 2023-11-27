using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    public bool AttRadyState;

    // 보스의 상태를 나타내는 Enum
    public enum BossState
    {
        Idle,  // 대기상태
        Idle1_2 // 추가된 상태
    }

    public Animator animator;   // 애니메이터 컴포넌트

    private BossState currentState; // 현재 보스의 상태

    private void Awake()
    {
        currentState = BossState.Idle;
        animator.Play("Idle");
    }
    // 시작할 때 초기 상태를 설정합니다.
    private void Start()
    {

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

    // AttRadyState가 true일 때 호출되는 함수
    public void ChangeToIdle1_2State()
    {
        if (currentState == BossState.Idle1_2)
            return;

        currentState = BossState.Idle1_2;

        // 애니메이터 컴포넌트의 파라미터를 설정하고 애니메이션 상태 전환을 합니다.
        animator.SetBool("IsAttReady", true);
    }
    // Update is called once per frame
    void Update()
    {
        if (AttRadyState)
            ChangeToIdle1_2State();
        else
            ChangeToIdleState();
    }
}