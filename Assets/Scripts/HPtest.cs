using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPtest : MonoBehaviour
{
    [Header("적 체력")]
    public int maxHealth;
    public int curHealth;

    public MeshRenderer[] meshs;

    // Start is called before the first frame update
    void Awake()
    {
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Attack weapon = other.GetComponent<Attack>();
            Debug.Log("Damage: " + weapon.damage);
            curHealth -= weapon.damage;

            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.red;

            StartCoroutine(ResetColorAfterDelay(0.5f));

            if (curHealth < 0) 
                curHealth = 0; // curHealth 값을 0으로 고정
        }
    }

    IEnumerator ResetColorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (curHealth > 0)
        {
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
    }


}
