using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int health;
    public Player player;

    public Image[] UIhealth;
    public Text UIStage;

    void Update()
    {
        
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.4f);
        }


        else
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.4f);

            //Player Die Effect
           // player.Die();

            //Result UI
            Debug.Log("ав╬З╫ю╢о╢ы!");
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Health Down
            HealthDown();
        }
    }

}
