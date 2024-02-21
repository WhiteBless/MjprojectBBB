using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_FadeOut : MonoBehaviour
{
    public float fadeSpeed = 0.5f; // 사라지는 속도

    void Update()
    {
        // 현재 스케일을 가져옴
        Vector3 currentScale = transform.localScale;

        // 서서히 스케일을 줄임
        float newScale = Mathf.Clamp01(currentScale.x - fadeSpeed * Time.deltaTime);
        Vector3 newScaleVector = new Vector3(newScale, newScale, newScale);

        // 변경된 스케일을 적용
        transform.localScale = newScaleVector;

        // 스케일이 0에 도달하면 오브젝트를 비활성화하여 완전히 사라지게 함
        if (newScale <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
