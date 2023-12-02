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

    void Update()
    {
        
    }

    public void HealthDown()
    {
        if (health > 1)
        {
            health--;

            UIhealth[health].color = new Color(1, 0, 0, 0.01f);
        }


        else
        {
            //All Health UI Off
            UIhealth[0].color = new Color(1, 0, 0, 0.01f);

            //Player Die Effect
           // player.Die();

            //Result UI
            Debug.Log("ав╬З╫ю╢о╢ы!");
        }

    }
}
