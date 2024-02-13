using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Razer : MonoBehaviour
{
    public int damage;
    [SerializeField]
    TrailRenderer TrRenderer;

    void Start()
    {
        TrRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(Show_False());
        }
    }


    IEnumerator Show_False()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("∑π¿Ã¿˙ ¥Í¿Ω");
        }
    }
}
