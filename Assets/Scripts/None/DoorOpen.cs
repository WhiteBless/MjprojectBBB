using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DoorOpen : MonoBehaviour
{
    public AudioSource doorSound; // ö�� ���带 ���� ����� �ҽ�
    public float openSpeed = 5f; // ö���� ������ �ӵ�

    private bool isOpening = false; // ö���� ���� ������ �ִ��� ���¸� ����

    void Update()
    {
        if (isOpening)
        {
            // ö���� ���� ����
            transform.Translate(Vector3.up * openSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorSound.Play(); // ö�� ������ ���� ���
            isOpening = true; // ö�� ���� ���·� ����
        }
    }
}