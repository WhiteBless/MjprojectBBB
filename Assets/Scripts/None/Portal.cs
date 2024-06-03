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
        // 포탈 효과음 재생
        GameManager.GMInstance.SoundManagerRef.PlaySFX(SoundManager.SFX.PORTAL);

        // 어쌔신이면
        if (GameManager.GMInstance.cur_Char == Define.Cur_Character.ASSASIN)
        {
            // 캐릭터 찾아와서 애니메이션 시작
            GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Animator>().SetTrigger("GoNext");
            // 캐릭터 찾아와서 애니메이션 시작
            GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>().moveSpeed_Discount =
                GameManager.GMInstance.Get_PlaySceneManager().CurCharacter.transform.GetChild(0).GetComponent<Assassin_Controller>().moveSpeed;
        }


        PD.Play();
    }
    
    public void Next_Scene()
    {
        SceneManager.LoadScene("Loading");
    }
}
