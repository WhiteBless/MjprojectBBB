using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int health;
    //public Player player;

    public Image[] UIhealth;

    public AudioSource audioSource; // 피격음을 재생할 AudioSource
    public AudioClip hitSound; // 피격음


    void Update()
    {
        
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.01f);

            // 피격음 재생
            audioSource.PlayOneShot(hitSound, 1.0f); // 피격음 재생 (볼륨은 1.0)
        }


        else
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.01f);

            //Player Die Effect
           // player.Die();

            //Result UI
            Debug.Log("죽었습니다!");
        }

    }
}
