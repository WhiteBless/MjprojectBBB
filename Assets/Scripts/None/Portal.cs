using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // ���� ����Ʈ�� ��Ÿ���� ������Ʈ

    public PlaySceneManager playSceneManager; // UI �Ŵ��� ����

    private void OnTriggerEnter(Collider other)
    {
        // �� �ڵ�� �۷ι� ��ǥ�� ����Ͽ� ������Ʈ�� ��ġ�� ȸ���� �����մϴ�.
        Transform objectTransform = other.transform;
        other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
        // ������Ʈ�� ��ġ�� ȸ���� toObj ������Ʈ�� �۷ι� ��ġ�� ȸ������ �����մϴ�.
        objectTransform.position = toObj.transform.position; // �۷ι� ��ġ ����
        objectTransform.rotation = toObj.transform.rotation; // �۷ι� ȸ�� ����
        other.gameObject.GetComponent<NavMeshAgent>().enabled = true;

        // TODO ## 1�� ��Ż ������
        if (this.gameObject.name == "1F_Portal")
        {
            // 1�� ������Ʈ ��Ȱ��ȭ
            GameManager.GMInstance.Get_PlaySceneManager().Stages[0].SetActive(false);
            // 2�� ������Ʈ Ȱ��ȭ
            GameManager.GMInstance.Get_PlaySceneManager().Stages[1].SetActive(true);
        }

        if (other.CompareTag("Player"))
        {
            playSceneManager.ActivateNextImage();
            playSceneManager.health = 3;
            playSceneManager.HealthDownActivateAll();
        }

    }
}
