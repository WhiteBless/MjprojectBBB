using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour
{
    [System.Serializable]
    public class BossPortalPair
    {
        public GameObject boss;
        public GameObject portal;
        public Transform warpPoint; // 포탈 사용시 이동할 위치
    }

    public List<BossPortalPair> bossPortalPairs;

    void Start()
    {
        // 게임 시작 시 모든 포탈 비활성화
        foreach (var pair in bossPortalPairs)
        {
            pair.portal.SetActive(false);
        }
    }

    void Update()
    {
        foreach (var pair in bossPortalPairs)
        {
            if (pair.boss == null || pair.portal == null || pair.warpPoint == null)
            {
                Debug.LogError("PortManager: BossPortalPair에 null 객체가 있습니다.");
                continue;
            }

            Boss_HP_Controller bossHpController = pair.boss.GetComponent<Boss_HP_Controller>();
            if (bossHpController == null)
            {
                Debug.LogError("PortManager: Boss_HP_Controller 컴포넌트를 찾을 수 없습니다.");
                continue;
            }

            if (bossHpController.isDead && !pair.portal.activeInHierarchy)
            {
                // 해당 보스가 죽었고, 포탈이 아직 활성화되지 않았다면 포탈 활성화
                pair.portal.SetActive(true);

                // 포탈 오브젝트에 Portal 스크립트가 있다면, 플레이어를 워프 포인트로 이동시키는 위치를 설정
                Portal portalScript = pair.portal.GetComponent<Portal>();
                if (portalScript != null)
                {
                    portalScript.toObj = pair.warpPoint.gameObject;
                }
                else
                {
                    Debug.LogError("PortManager: Portal 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }
    }
}

