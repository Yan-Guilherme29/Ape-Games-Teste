using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 5f;
    public float offsetY = 2f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = new Vector3(
            player.position.x,
            player.position.y + offsetY,
            -10f
        );

        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }
}