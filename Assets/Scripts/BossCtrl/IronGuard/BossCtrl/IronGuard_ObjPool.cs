using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGuard_ObjPool : MonoBehaviour
{
    public GameObject Down_3Combo_objectPrefab; // 9번 찍기 프리팹
    public GameObject SpiritSword_objectPrefab; // 검기 프리팹
    public GameObject JumpAtk_objectPrefab; // 점프 공격 프리팹

    public int poolSize = 5; // 풀 크기
    [SerializeField]
    private List<GameObject> Down_3CombpVFX_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> SpiritSword_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> JumpAtk_objectPool; // 오브젝트 풀

    [SerializeField]
    Transform DownEffect_Parent;
    [SerializeField]
    Transform Spirit_Parent;
    [SerializeField]
    Transform JumpAtk_Parent;

    private void Start()
    {
        Down_3CombpVFX_objectPool = new List<GameObject>();
        SpiritSword_objectPool = new List<GameObject>();
        JumpAtk_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Down_3Combo_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = DownEffect_Parent;
            Down_3CombpVFX_objectPool.Add(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(SpiritSword_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = Spirit_Parent;
            SpiritSword_objectPool.Add(obj);
        }

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(JumpAtk_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = JumpAtk_Parent;
            JumpAtk_objectPool.Add(obj);
        }
    }

    // 오브젝트를 풀에서 가져오는 메서드
    public GameObject GetObjectFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < Down_3CombpVFX_objectPool.Count; i++)
        {
            if (!Down_3CombpVFX_objectPool[i].activeInHierarchy)
            {
                Down_3CombpVFX_objectPool[i].SetActive(true);
                return Down_3CombpVFX_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(Down_3Combo_objectPrefab);
        newObj.SetActive(true);
        Down_3CombpVFX_objectPool.Add(newObj);
        return newObj;
    }

    public GameObject Get_SpiritSword_ObjectFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < SpiritSword_objectPool.Count; i++)
        {
            if (!SpiritSword_objectPool[i].activeInHierarchy)
            {
                SpiritSword_objectPool[i].SetActive(true);
                return SpiritSword_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(SpiritSword_objectPrefab);
        newObj.SetActive(true);
        SpiritSword_objectPool.Add(newObj);
        return newObj;
    }

    public GameObject Get_JumpAtk_ObjectFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < JumpAtk_objectPool.Count; i++)
        {
            if (!JumpAtk_objectPool[i].activeInHierarchy)
            {
                JumpAtk_objectPool[i].SetActive(true);
                return JumpAtk_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(JumpAtk_objectPrefab);
        newObj.SetActive(true);
        JumpAtk_objectPool.Add(newObj);
        return newObj;
    }

    // 오브젝트를 풀에 반환하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
