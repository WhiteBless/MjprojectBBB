using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class Portal : MonoBehaviour
{
    public GameObject toObj; // 워프 포인트를 나타내는 오브젝트

    public PlaySceneManager playSceneManager; // UI 매니저 참조
    [SerializeField]
    PlayableDirector PD;


    private void OnTriggerEnter(Collider other)
    {
        PD.Play();
    }
    
    public void Next_Scene()
    {
        SceneManager.LoadScene("Loading");
    }
}
