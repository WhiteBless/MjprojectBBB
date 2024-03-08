using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ChangeSceneByButtonTag(button));
        }
    }

    private void ChangeSceneByButtonTag(Button button)
    {
        string tag = button.gameObject.tag;

        switch (tag)
        {
            case "Start":
                SceneManager.LoadScene("Loading");
                break;
            default:
                Debug.LogError("Invalid button tag: " + tag);
                break;
        }
    }
}
