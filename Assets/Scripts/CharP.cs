using UnityEngine;

public class CharP : MonoBehaviour
{
    public Transform player;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;

    private Vector3 currentRotation; // 현재 오일러 각도를 저장할 변수

    // LateUpdate is called after all Update functions have been called.
    void LateUpdate()
    {
        // 디버깅 정보 출력
        Debug.Log($"Player Position: {player.position}");
        Debug.Log($"Position Offset: {positionOffset}");

        // 월드 좌표계로 변환된 오프셋을 사용하여 플레이어의 위치에 더합니다.
        transform.position = player.TransformPoint(positionOffset);
        Debug.Log($"New Position: {transform.position}");

        // 플레이어를 바라보도록 회전을 조절
        transform.LookAt(player.position);

        // 오일러 각도를 저장하고 새로운 각도를 더한 후에 적용
        currentRotation += rotationOffset;
        transform.rotation *= Quaternion.Euler(currentRotation);

        Debug.Log($"New Rotation: {transform.rotation.eulerAngles}");
    }
}