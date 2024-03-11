using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CaneraController : MonoBehaviour
{

    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, 5.0f);

    [SerializeField]
    GameObject _player = null;

    public float wallDistance = 2.0f; // ������ �Ÿ�


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // �Ǵ� _player�� �÷��̾� ������Ʈ�� ���� �Ҵ��ص� �˴ϴ�.
    }

    // �� �����Ӹ��� ���� 
    void LateUpdate()
    {
        if (!(SceneManager.GetActiveScene().name == "Main"))
        {
            transform.position = _player.transform.position + _delta;
            transform.LookAt(_player.transform);
        }

        //if (_mode == Define.CameraMode.QarterView && _player != null)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
        //    {
        //        float dist = (hit.point - _player.transform.position).magnitude * wallDistance;
        //        transform.position = _player.transform.position + _delta.normalized * dist;
        //    }
        //    else
        //    {
        //        transform.position = _player.transform.position + _delta;
        //        transform.LookAt(_player.transform);
        //    }
        //}
    }
    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QarterView;
        _delta = delta;
    }
}