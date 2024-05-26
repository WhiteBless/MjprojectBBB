using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treant_ObjPool : MonoBehaviour
{
    public GameObject LeafPlace_objectPrefab; // LeafPlace
    public GameObject LeafPlace_Guide_objectPrefab; 

    public int poolSize; // 풀 크기
    [SerializeField]
    private List<GameObject> LeafPlace_objectPool; // 오브젝트 풀
    [SerializeField]
    private List<GameObject> LeafPlace_Guide_objectPool; // 오브젝트 풀

    [SerializeField]
    Transform LeafPlace_Parent;
    [SerializeField]
    Transform LeafPlace_Guide_Parent;
    // Start is called before the first frame update
    void Start()
    {
        LeafPlace_objectPool = new List<GameObject>();
        LeafPlace_Guide_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            // LeafPlace 오브젝트 풀 생성
            GameObject obj_1 = Instantiate(LeafPlace_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = LeafPlace_Parent;
            LeafPlace_objectPool.Add(obj_1);

            // LeafPlace_Guide 오브젝트 풀 생성
            GameObject obj_2 = Instantiate(LeafPlace_Guide_objectPrefab);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = LeafPlace_Guide_Parent;
            LeafPlace_Guide_objectPool.Add(obj_2);
        }
    }

    public GameObject GetLeafPlaceFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < LeafPlace_objectPool.Count; i++)
        {
            if (!LeafPlace_objectPool[i].activeInHierarchy)
            {
                LeafPlace_objectPool[i].SetActive(true);
                return LeafPlace_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(LeafPlace_objectPrefab);
        newObj.SetActive(true);
        LeafPlace_objectPool.Add(newObj);
        newObj.transform.parent = LeafPlace_Parent;
        return newObj;
    }

    public GameObject GetLeafPlace_Guide_FromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < LeafPlace_Guide_objectPool.Count; i++)
        {
            if (!LeafPlace_Guide_objectPool[i].activeInHierarchy)
            {
                LeafPlace_Guide_objectPool[i].SetActive(true);
                return LeafPlace_Guide_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(LeafPlace_Guide_objectPrefab);
        newObj.SetActive(true);
        LeafPlace_Guide_objectPool.Add(newObj);
        newObj.transform.parent = LeafPlace_Guide_Parent;
        return newObj;
    }
}