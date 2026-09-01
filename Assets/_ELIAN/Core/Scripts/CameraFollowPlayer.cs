using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] public GameObject Player;

    [Header("Limites horizontales")]
    [SerializeField] private float minX = 10f;
    [SerializeField] private float maxX = 30f;

    [Header("Posicion vertical")]
    [SerializeField] private float fixedY = 3.1875f;

    private void LateUpdate()
    {
        if (Player == null)
            return;

        float cameraX = Mathf.Clamp(
            Player.transform.position.x,
            minX,
            maxX
        );

        transform.position = new Vector3(
            cameraX,
            fixedY,
            transform.position.z
        );
    }
}
