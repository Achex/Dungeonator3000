using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public GameObject cube;
    public float maxDistance = 5f; 
    private bool inPNCMode;

    void Start()
    {
        inPNCMode = PlayerPrefs.GetString("GameMode").Equals("PNC");
        if (inPNCMode)
        {
            cube.SetActive(false);
            transform.position = new Vector3(0,12,0);
        }
    }



    void Update()
    {
        if (!inPNCMode && player != null)
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
