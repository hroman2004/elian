using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    public GameObject Player;

    void LateUpdate()
    {
        Vector3 newPos = Player.transform.position;
        newPos.z = transform.position.z;

        transform.position = newPos;
    }
}
