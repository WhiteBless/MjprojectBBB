using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPtest : MonoBehaviour
{
    [Header("Àû Ã¼·Â")]
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
        if (other.tag == "Melee")
        {
            Attack weapon = other.GetComponent<Attack>();
            Debug.Log("Damage: " + weapon.damage);
            curHealth -= weapon.damage;

            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = Color.red;

            StartCoroutine(ResetColorAfterDelay(0.1f));
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
