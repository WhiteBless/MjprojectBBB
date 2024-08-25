using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Samurai_ObjPool : MonoBehaviour
{
    public GameObject mouseMoveEffect;         // 마우스 클릭 오브젝트

    public int poolSize; // 풀 크기

    [SerializeField]
    public List<GameObject> mouseMoveEffect_objectPool; // 오브젝트 풀


    [SerializeField]
    Transform mouseMoveEffect_Parent;

    // Start is called before the first frame update
    void Start()
    {
        mouseMoveEffect_objectPool = new List<GameObject>();
        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            // 마우스 클릭 이동 이펙트
            GameObject obj_3 = Instantiate(mouseMoveEffect);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = mouseMoveEffect_Parent;
            mouseMoveEffect_objectPool.Add(obj_3);
        }
    }

    public GameObject MouseMoveEffectFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < mouseMoveEffect_objectPool.Count; i++)
        {
            if (!mouseMoveEffect_objectPool[i].activeInHierarchy)
            {
                mouseMoveEffect_objectPool[i].SetActive(true);
                return mouseMoveEffect_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(mouseMoveEffect);
        newObj.SetActive(true);
        mouseMoveEffect_objectPool.Add(newObj);
        newObj.transform.parent = mouseMoveEffect_Parent;
        return newObj;
    }
}
