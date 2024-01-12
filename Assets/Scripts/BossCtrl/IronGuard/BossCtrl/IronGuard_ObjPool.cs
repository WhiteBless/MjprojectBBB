using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronGuard_ObjPool : MonoBehaviour
{
    public GameObject Down_3Combo_objectPrefab; // 9번 찍기 프리팹
    public GameObject Razer_objectPrefab; // 레이저 프리팹

    public int poolSize = 10; // 풀 크기
    [SerializeField]
    private List<GameObject> Down_3CombpVFX_objectPool; // 오브젝트 풀
    
    [SerializeField]
    Transform DownEffect_Parent;
    [SerializeField]
    Transform Razer_Parent;

    private void Start()
    {
        Down_3CombpVFX_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(Down_3Combo_objectPrefab);
            obj.SetActive(false);
            obj.transform.parent = DownEffect_Parent;
            Down_3CombpVFX_objectPool.Add(obj);
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

    // 오브젝트를 풀에 반환하는 메서드
    public void ReturnObjectToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
