using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_ObjPool : MonoBehaviour
{
    public GameObject shuriken_objectPrefab; // 수리검 발사체
    
    public int poolSize; // 풀 크기
    [SerializeField]
    private List<GameObject> shuriken_objectPool; // 오브젝트 풀

    [SerializeField]
    Transform shuriken_Parent;

    // Start is called before the first frame update
    void Start()
    {
        shuriken_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj_1 = Instantiate(shuriken_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = shuriken_Parent;
            shuriken_objectPool.Add(obj_1);
        }
    }

    public GameObject ShurikenFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < shuriken_objectPool.Count; i++)
        {
            if (!shuriken_objectPool[i].activeInHierarchy)
            {
                shuriken_objectPool[i].SetActive(true);
                return shuriken_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(shuriken_objectPrefab);
        newObj.SetActive(true);
        shuriken_objectPool.Add(newObj);
        newObj.transform.parent = shuriken_Parent;
        return newObj;
    }
}
