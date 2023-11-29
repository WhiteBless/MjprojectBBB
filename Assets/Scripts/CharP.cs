using UnityEngine;

public class CharP : MonoBehaviour
{
    public Transform player; // player 오브젝트의 Transform 컴포넌트를 Inspector 창에서 할당해주세요.
    public Vector3 offset; // player와 character 사이의 거리를 설정합니다.

    // Update is called once per frame
    void Update()
    {
        // player 오브젝트의 현재 위치에 offset을 더하여 Character 오브젝트의 위치를 설정합니다.
        transform.position = player.position + offset;
    }
}