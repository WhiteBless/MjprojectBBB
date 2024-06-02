using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFlag : MonoBehaviour
{ 
    void Start()
    {
        // 깃발 오브젝트의 Renderer 컴포넌트를 찾습니다.
        Renderer flagRenderer = GetComponent<Renderer>();

        // 진한 녹색으로 색상을 설정합니다. RGB 값으로 진한 녹색을 정의할 수 있습니다.
        // 예를 들어, 진한 녹색의 RGB 값은 (0, 100, 0)이 될 수 있습니다.
        Color darkGreen = new Color(0f, 0.392f, 0f);

        // 오브젝트의 머티리얼 색상을 진한 녹색으로 변경합니다.
        flagRenderer.material.color = darkGreen;
    }
}
