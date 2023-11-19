using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;
    public Text loadtext;

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync("Play");//""에 넘어갈 씬 이름으로 변경
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if (progressbar.value < 1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }

            else
            {
                loadtext.text = "Let's Started";
                SceneManager.LoadScene("Play");//""에 넘어갈 씬 이름으로 변경
            }

            if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}


