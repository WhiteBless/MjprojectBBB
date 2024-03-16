using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    public float moveSpeed; // �̵� �ӵ�
    public float maxDistance; // �ִ� �̵� �Ÿ�

    void Start()
    {
        
    }

    
    void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // �߻�ü �̵� �Ÿ� üũ
        if (Vector3.Distance(transform.position, transform.parent.parent.parent.GetChild(1).position) > maxDistance)
        {
            gameObject.SetActive(false); // ���� �Ÿ� �̵� �� ��Ȱ��ȭ
        }
    }
}
