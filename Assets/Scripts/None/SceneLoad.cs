using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.GMInstance.SoundManagerRef.PlayBGM(SoundManager.BGM.LOADING);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.NONE)
        {
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Main");//""에 넘어갈 씬 이름으로 변경
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
                    SceneManager.LoadScene("Main");//""에 넘어갈 씬 이름으로 변경
                }

                if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }


        // 1층으로 이동
        if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.MAIN)
        {
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Play_Floor_1F");//""에 넘어갈 씬 이름으로 변경
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                progressbar.value = progress;

                if (operation.progress >= 0.9f)
                {
                    progressbar.value = 1f;

                    yield return new WaitForSeconds(1.0f);

                    // 씬 전환을 바로 트리거
                    operation.allowSceneActivation = true;
                }

                //yield return null;
                //if (progressbar.value < 1f)
                //{
                //    progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
                //}

                //else
                //{
                //    SceneManager.LoadScene("Play_Floor_1F");//""에 넘어갈 씬 이름으로 변경
                //}

                //if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
                //{
                //    operation.allowSceneActivation = true;
                //}
                yield return null;
            }
        }



        // 2층으로 이동
        if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.FLOOR_1)
        {
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Play_Floor_2F");//""에 넘어갈 씬 이름으로 변경
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
                    SceneManager.LoadScene("Play_Floor_2F");//""에 넘어갈 씬 이름으로 변경
                }

                if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }


        // 3층으로 이동
        if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.FLOOR_2)
        {
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Play_Floor_3F");//""에 넘어갈 씬 이름으로 변경
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
                    SceneManager.LoadScene("Play_Floor_3F");//""에 넘어갈 씬 이름으로 변경
                }

                if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }


        // 4층으로 이동
        if (GameManager.GMInstance.cur_Scene == Define.Cur_Scene.FLOOR_3)
        {
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Play_Floor_4F");//""에 넘어갈 씬 이름으로 변경
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
                    SceneManager.LoadScene("Play_Floor_4F");//""에 넘어갈 씬 이름으로 변경
                }

                if (Input.touchCount > 0 && progressbar.value >= 1f && operation.progress >= 0.9f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }
    }
}


