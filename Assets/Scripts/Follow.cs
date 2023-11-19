using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraController : MonoBehaviour
{

    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, 5.0f);

    [SerializeField]
    GameObject _player = null;


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // 또는 _player에 플레이어 오브젝트를 직접 할당해도 됩니다.
    }

    // 매 프레임마다 갱신 
    void LateUpdate()
    {
        if (_mode == Define.CameraMode.QarterView && _player != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }
    public void SetQuaterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QarterView;
        _delta = delta;
    }
}