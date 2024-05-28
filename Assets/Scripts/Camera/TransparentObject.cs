//using UnityEngine;

//public class TransparentObject : MonoBehaviour
//{
//    public Transform player; // 플레이어 오브젝트
//    public float transparency = 0.5f; // 반투명도 조절을 위한 변수

//    void Update()
//    {
//        // 카메라와 플레이어 사이의 방향 벡터
//        Vector3 direction = player.position - transform.position;

//        // 카메라에서 플레이어까지의 거리
//        float distanceToPlayer = direction.magnitude;

//        // 카메라에서 플레이어까지의 레이캐스트
//        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);

//        foreach (RaycastHit hit in hits)
//        {
//            Renderer renderer = hit.transform.GetComponent<Renderer>();
//            if (renderer != null)
//            {
//                // MeshRenderer에서 모든 Material 가져오기
//                Material[] materials = renderer.materials;

//                // 모든 Material 반투명하게 만들기
//                foreach (Material material in materials)
//                {
//                    // 반투명도 설정
//                    material.SetFloat("_Surface", 1); // 1은 Transparent를 의미합니다.
//                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
//                    material.SetInt("_ZWrite", 0);
//                    material.DisableKeyword("_ALPHATEST_ON");
//                    material.EnableKeyword("_ALPHABLEND_ON");
//                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
//                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

//                    // 머티리얼의 색상을 가져와 알파 값을 설정합니다.
//                    Color color = material.color;
//                    color.a = transparency;
//                    material.color = color;
//                }
//            }
//        }
//    }
//}

using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public Transform player; // 플레이어 오브젝트
    public float transparency = 0.5f; // 반투명도 조절을 위한 변수

    private bool isTransparent = false; // 오브젝트가 반투명 상태인지 여부를 나타내는 변수
    private Material[] originalMaterials; // 오브젝트의 원래 머티리얼을 저장할 배열

    void Update()
    {
        // 카메라와 플레이어 사이의 방향 벡터
        Vector3 direction = player.position - transform.position;

        // 카메라에서 플레이어까지의 거리
        float distanceToPlayer = direction.magnitude;

        // 카메라에서 플레이어까지의 레이캐스트
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distanceToPlayer);

        bool hitTransparentObject = false; // 레이가 반투명 오브젝트에 닿았는지 여부를 나타내는 변수

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            if (renderer != null) // 오브젝트 자체에 레이가 닿았을 때
            {
                originalMaterials = renderer.materials;
                SetObjectTransparent(renderer);
                hitTransparentObject = true;
            }
        }

        // 레이가 반투명 오브젝트에 닿지 않은 경우 불투명 상태로 되돌리기
        if (!hitTransparentObject && isTransparent)
        {
            SetObjectOpaque();
        }
    }

    // 오브젝트를 반투명하게 만드는 함수
    void SetObjectTransparent(Renderer renderer)
    {
        Material[] materials = renderer.materials;
        foreach (Material material in materials)
        {
            material.SetFloat("_Surface", 1); // 1은 Transparent를 의미합니다.
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            Color color = material.color;
            color.a = transparency; // 반투명도 설정
            material.color = color;
        }
        isTransparent = true; // 반투명 상태로 변경
    }

    // 오브젝트를 불투명하게 만드는 함수
    void SetObjectOpaque()
    {
        Material[] materials = originalMaterials; // 모든 머티리얼 배열 가져오기
        foreach (Material material in materials)
        {
            material.SetFloat("_Surface", 0); // 0은 Opaque를 의미합니다.
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            Color color = material.color;
            color.a = 1f; // 불투명도 설정
            material.color = color;
        }
        isTransparent = false; // 불투명 상태로 변경
    }
}


