using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Follow : MonoBehaviour
{

    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, 5.0f);

    [SerializeField]
    GameObject _player = null;

    public float wallDistance = 2.0f; // 벽과의 거리


    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        // 또는 _player에 플레이어 오브젝트를 직접 할당해도 됩니다.
    }

    // 매 프레임마다 갱신 
    void Update()
    {

        //if (!(SceneManager.GetActiveScene().name == ""))
        //{
           
        //}

        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);

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