using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assassin_ObjPool : MonoBehaviour
{
    public GameObject shuriken_objectPrefab_Q; // 수리검 발사체
    public GameObject shuriken_objectPrefab_W; // 수리검 발사체
    public GameObject mouseMoveEffect;         // 마우스 클릭 오브젝트

    public int poolSize; // 풀 크기
    [SerializeField]
    private List<GameObject> shuriken_objectPool_Q; // 오브젝트 풀
    [SerializeField]
    public List<GameObject> shuriken_objectPool_W; // 오브젝트 풀
    [SerializeField]
    public List<GameObject> mouseMoveEffect_objectPool; // 오브젝트 풀

    [SerializeField]
    Transform shuriken_Parent_Q;
    [SerializeField]
    public Transform shuriken_Parent_W;
    [SerializeField]
    Transform mouseMoveEffect_Parent;

    // Start is called before the first frame update
    void Start()
    {
        shuriken_objectPool_Q = new List<GameObject>();
        shuriken_objectPool_W = new List<GameObject>();
        mouseMoveEffect_objectPool = new List<GameObject>();

        // 초기에 풀에 오브젝트를 생성하여 저장
        for (int i = 0; i < poolSize; i++)
        {
            // Q스킬 수리검 생성
            GameObject obj_1 = Instantiate(shuriken_objectPrefab_Q);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = shuriken_Parent_Q;
            shuriken_objectPool_Q.Add(obj_1);
            // W스킬 수리검 생성
            GameObject obj_2 = Instantiate(shuriken_objectPrefab_W);
            obj_2.transform.Rotate(Vector3.zero);
            obj_2.SetActive(false);
            obj_2.transform.parent = shuriken_Parent_W;
            shuriken_objectPool_W.Add(obj_2);
            // 마우스 클릭 이동 이펙트
            GameObject obj_3 = Instantiate(mouseMoveEffect);
            obj_3.transform.Rotate(Vector3.zero);
            obj_3.SetActive(false);
            obj_3.transform.parent = mouseMoveEffect_Parent;
            mouseMoveEffect_objectPool.Add(obj_3);
        }
    }

    public GameObject ShurikenFromPool_Q()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < shuriken_objectPool_Q.Count; i++)
        {
            if (!shuriken_objectPool_Q[i].activeInHierarchy)
            {
                shuriken_objectPool_Q[i].SetActive(true);
                return shuriken_objectPool_Q[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(shuriken_objectPrefab_Q);
        newObj.SetActive(true);
        shuriken_objectPool_Q.Add(newObj);
        newObj.transform.parent = shuriken_Parent_Q;
        return newObj;
    }

    public GameObject ShurikenFromPool_W()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < shuriken_objectPool_W.Count; i++)
        {
            if (!shuriken_objectPool_W[i].activeInHierarchy)
            {
                shuriken_objectPool_W[i].SetActive(true);
                return shuriken_objectPool_W[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(shuriken_objectPrefab_W);
        newObj.SetActive(true);
        shuriken_objectPool_W.Add(newObj);
        newObj.transform.parent = shuriken_Parent_W;
        return newObj;
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
