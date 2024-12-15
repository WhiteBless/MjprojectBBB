using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_ObjPool : MonoBehaviour
{
    public int poolSize; // 풀 크기

    [Header("----Normal_WindAtk_---")]
    public GameObject WindAtk_objectPrefab;
    [SerializeField]
    private List<GameObject> WindAtk_objectPool; // 오브젝트 풀
    [SerializeField]
    Transform WindAtk_Parent;

    [Header("----Fire_FuryAtk_---")]
    public GameObject Fury_objectPrefab;
    [SerializeField]
    private List<GameObject> Fury_objectPool; // 오브젝트 풀
    [SerializeField]
    Transform Fury_Parent;

    [Header("----Fire_FireBall---")]
    public GameObject FireBall_objectPrefab;
    [SerializeField]
    private List<GameObject> FireBall_objectPool; // 오브젝트 풀
    [SerializeField]
    Transform FireBall_Parent;

    // Start is called before the first frame update
    void Start()
    {
        WindAtk_objectPool = new List<GameObject>();
        Fury_objectPool = new List<GameObject>();
        FireBall_objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            // WindAtk 오브젝트 풀 생성
            GameObject obj_1 = Instantiate(WindAtk_objectPrefab);
            obj_1.transform.Rotate(Vector3.zero);
            obj_1.SetActive(false);
            obj_1.transform.parent = WindAtk_Parent;
            WindAtk_objectPool.Add(obj_1);
        }

        for (int i = 0; i < poolSize; i++)
        {
            // Fury 오브젝트 풀 생성
            GameObject obj_2 = Instantiate(Fury_objectPrefab);
            obj_2.transform.Rotate(Vector3.zero);
            // obj_2.SetActive(false);
            obj_2.transform.parent = Fury_Parent;
            Fury_objectPool.Add(obj_2);
        }

        for (int i = 0; i < poolSize; i++)
        {
            // FireBall 오브젝트 풀 생성
            GameObject obj_3 = Instantiate(FireBall_objectPrefab);
            obj_3.transform.localRotation = Quaternion.Euler(Vector3.zero);
            // obj_3.SetActive(false);
            obj_3.transform.parent = FireBall_Parent;
            FireBall_objectPool.Add(obj_3);
        }
    }

    #region WindAtk
    public GameObject GetWindAtkFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < WindAtk_objectPool.Count; i++)
        {
            if (!WindAtk_objectPool[i].activeInHierarchy)
            {
                WindAtk_objectPool[i].SetActive(true);
                return WindAtk_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(WindAtk_objectPrefab);
        newObj.SetActive(true);
        WindAtk_objectPool.Add(newObj);
        newObj.transform.parent = WindAtk_Parent;
        return newObj;
    }
    #endregion

    #region FuryAtk
    public GameObject GetFuryAtkFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < Fury_objectPool.Count; i++)
        {
            if (!Fury_objectPool[i].activeInHierarchy)
            {
                Fury_objectPool[i].SetActive(true);
                return Fury_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(Fury_objectPrefab);
        newObj.SetActive(true);
        Fury_objectPool.Add(newObj);
        newObj.transform.parent = Fury_Parent;
        return newObj;
    }
    #endregion

    #region FireBall
    public GameObject GetFireBallAtkFromPool()
    {
        // 비활성화된 오브젝트를 찾아 반환
        for (int i = 0; i < FireBall_objectPool.Count; i++)
        {
            if (!FireBall_objectPool[i].activeInHierarchy)
            {
                FireBall_objectPool[i].SetActive(true);
                return FireBall_objectPool[i];
            }
        }

        // 모든 오브젝트가 사용 중일 경우 새로운 오브젝트 생성 후 반환
        GameObject newObj = Instantiate(FireBall_objectPrefab);
        newObj.SetActive(true);
        FireBall_objectPool.Add(newObj);
        newObj.transform.parent = FireBall_Parent;
        return newObj;
    }
    #endregion
}
