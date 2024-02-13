using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 5f; 

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPos = player.position;

            //force the camera stays within the specified range
            float clampedX = Mathf.Clamp(playerPos.x, -maxDistance, maxDistance);
            float clampedZ = Mathf.Clamp(playerPos.z, -maxDistance, maxDistance);

            //set the camera's position based on the player's position within the allowed range
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
        }
    }
}
