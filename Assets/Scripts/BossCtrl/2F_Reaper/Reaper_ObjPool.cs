using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Reaper_ObjPool : MonoBehaviour
{
    public GameObject DarkDecline_objectPrefab; // 어둠의쇠락
    public GameObject DarkDecline2_objectPrefab; // 어둠의쇠락 각성
    public GameObject DarkHand_objectPrefab; // 어둠의손짓
    public GameObject DarkHand2_objectPrefab; // 어둠의손짓2
    public GameObject DarkBall_objectPrefab; // 어둠의손짓2

    public int poolSize; // 풀 크기
    [SerializeField]
    private List<GameObject> DarkDecline_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> DarkDecline2_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> DarkHand_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> DarkHand2_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> DarkBall_objectPool; // 오브젝트 풀


    [SerializeField]
    Transform DarkDecline_Parent;
    [SerializeField]
    Transform DarkDecline2_Parent;
    [SerializeField]
    Transform DarkHand_Parent;
    [SerializeField]
    Transform DarkHand2_Parent;
    [SerializeField]
    Transform DarkBall_Parent;

    // Start is called before the first frame update
    void Start()
    {
        DarkDecline_objectPool = new List<GameObject>();
        DarkHand_objectPool = new List<GameObject>();
        DarkHand2_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            // 어둠의 쇠락 오브젝트 풀 생성
            GameObject obj_1 = Instantiate(DarkDecline_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = DarkDecline_Parent;
            DarkDecline_objectPool.Add(obj_1);
            
            // 어둠의 손짓 오브젝트 풀 생성
            GameObject obj_2 = Instantiate(DarkHand_objectPrefab);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = DarkHand_Parent;
            DarkHand_objectPool.Add(obj_2);

            // 어둠의 손짓 오브젝트 풀 생성
            GameObject obj_3 = Instantiate(DarkHand2_objectPrefab);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = DarkHand2_Parent;
            DarkHand2_objectPool.Add(obj_3);

            // 어둠의 쇠락2 오브젝트 풀 생성
            GameObject obj_4 = Instantiate(DarkDecline2_objectPrefab);
            obj_4.transform.Rotate(Vector3.zero);
            obj_4.SetActive(false);
            obj_4.transform.parent = DarkDecline2_Parent;
            DarkDecline2_objectPool.Add(obj_4);

            // 어둠의 구체 오브젝트 풀 생성
            GameObject obj_5 = Instantiate(DarkBall_objectPrefab);
            // 구체의 타켓 설정
            // obj_5.GetComponent<DarkBall_Ctrl>().Target = GameObject.FindWithTag("Player");
            // 구체 속성
            obj_5.GetComponent<DarkBall_Ctrl>().ballColor = Reaper_Pattern_Color.NORMAL;
            obj_5.transform.Rotate(Vector3.zero);
            obj_5.SetActive(false);
            obj_5.transform.parent = DarkBall_Parent;
            DarkBall_objectPool.Add(obj_5);
        }
    }

        //// 초기에 풀에 오브젝트를 생성하여 저장
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj_2 = Instantiate(DarkHand_objectPrefab);
        //    obj_2.transform.Rotate(Vector3.zero);
        //    obj_2.SetActive(false);
        //    obj_2.transform.parent = DarkHand_Parent;
        //    DarkHand_objectPool.Add(obj_2);
        //}

        //// 초기에 풀에 오브젝트를 생성하여 저장
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject obj_3 = Instantiate(DarkHand2_objectPrefab);
        //    obj_3.transform.Rotate(Vector3.zero);
        //    obj_3.SetActive(false);
        //    obj_3.transform.parent = DarkHand2_Parent;
        //    DarkHand2_objectPool.Add(obj_3);
        //}

    public GameObject GetDarkDeclineFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < DarkDecline_objectPool.Count; i++)
        {
            if (!DarkDecline_objectPool[i].activeInHierarchy)
            {
                DarkDecline_objectPool[i].SetActive(true);
                return DarkDecline_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(DarkDecline_objectPrefab);
        newObj.SetActive(true);
        DarkDecline_objectPool.Add(newObj);
        newObj.transform.parent = DarkDecline_Parent;
        return newObj;
    }

    public GameObject GetDarkDecline2FromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < DarkDecline2_objectPool.Count; i++)
        {
            if (!DarkDecline2_objectPool[i].activeInHierarchy)
            {
                DarkDecline2_objectPool[i].SetActive(true);
                return DarkDecline2_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(DarkDecline2_objectPrefab);
        newObj.SetActive(true);
        DarkDecline2_objectPool.Add(newObj);
        newObj.transform.parent = DarkDecline2_Parent;
        return newObj;
    }

    public GameObject GetDarkHandFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < DarkHand_objectPool.Count; i++)
        {
            if (!DarkHand_objectPool[i].activeInHierarchy)
            {
                DarkHand_objectPool[i].SetActive(true);
                return DarkHand_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(DarkHand_objectPrefab);
        newObj.SetActive(true);
        DarkHand_objectPool.Add(newObj);
        newObj.transform.parent = DarkHand_Parent;
        return newObj;
    }

    public GameObject GetDarkHand2FromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < DarkHand2_objectPool.Count; i++)
        {
            if (!DarkHand2_objectPool[i].activeInHierarchy)
            {
                DarkHand2_objectPool[i].SetActive(true);
                return DarkHand2_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(DarkHand2_objectPrefab);
        newObj.SetActive(true);
        DarkHand2_objectPool.Add(newObj);
        newObj.transform.parent = DarkHand2_Parent;
        return newObj;
    }

    public GameObject GetDarkBallFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < DarkBall_objectPool.Count; i++)
        {
            if (!DarkBall_objectPool[i].activeInHierarchy)
            {
                DarkBall_objectPool[i].SetActive(true);
                return DarkBall_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(DarkBall_objectPrefab);
        newObj.SetActive(true);
        // newObj.GetComponent<DarkBall_Ctrl>().Target = GameObject.FindWithTag("Player");
        newObj.GetComponent<DarkBall_Ctrl>().ballColor = Reaper_Pattern_Color.NORMAL;
        DarkBall_objectPool.Add(newObj);
        newObj.transform.parent = DarkBall_Parent;
        return newObj;
    }
}
