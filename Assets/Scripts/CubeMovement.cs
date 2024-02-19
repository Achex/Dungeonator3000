using UnityEngine;
using UnityEngine.SceneManagement;
using static MazeGen;
using static InstallWalls;

public class CubeMovement : MonoBehaviour
{
    private Vector3 cubePosition;
    [Header("Cube Settings")]
    public float cubeSpeed = 0.5f;
    public float rotationSpeed = 12f;

    public bool EnableW = true;
    public bool EnableA = true;
    public bool EnableS = true;
    public bool EnableD = true;

    public Canvas loadingScreen;

    void Start()
    {
        string flattenedString = PlayerPrefs.GetString("MazeGrid");

        // Split the string into individual rows
        string[] rowStrings = flattenedString.Split(';');

        // Convert row strings back to integer arrays
        int[][] mazeGrid = new int[rowStrings.Length][];
        for (int i = 0; i < rowStrings.Length; i++)
        {
            string[] stringValues = rowStrings[i].Split(',');
            mazeGrid[i] = new int[stringValues.Length];

            for (int j = 0; j < stringValues.Length; j++)
            {
                mazeGrid[i][j] = int.Parse(stringValues[j]);
            }
        }
        
        Install(mazeGrid);
        cubePosition = this.transform.position;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        MoveCube(movement);
    }

    void MoveCube(Vector3 movement)
    {
        Vector3 newPosition = cubePosition + movement * (cubeSpeed / 10);
        //check for collisions before updating the position
        if (!IsColliding(newPosition))
        { 
            // cubePosition = newPosition;
            // transform.position = cubePosition;



            cubePosition = newPosition;

            Vector3 lookDirection = movement.normalized;
            lookDirection.y = 0f;

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);

                // Apply a fixed rotation to compensate for the 90-degree anticlockwise offset
                Quaternion fixedRotation = Quaternion.Euler(0f, 90f, 0f);
                targetRotation *= fixedRotation;

                // Use Quaternion.Slerp to interpolate between current rotation and target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            transform.position = cubePosition;
        }
    }

    bool IsColliding(Vector3 position)
    {
        float radius = transform.localScale.x / 2; // <----- adjust the radius according to cube size

        RaycastHit hit;
        Vector3 direction = position - cubePosition;
        float distance = Vector3.Distance(cubePosition, position);

        //cast a sphere along the movement direction
        if (Physics.SphereCast(cubePosition, radius, direction, out hit, distance))
        {
            if (hit.collider.CompareTag("maze")) 
            {
                return true;
            }
            else if (hit.collider.CompareTag("goal"))
            {
                PlayerPrefs.SetString("State", "Repeat");
                //loadingScreen.gameObject.SetActive(true);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

                Debug.Log("well done!");
            }

        }

        return false;
    }
}



// using UnityEngine;
// using static MazeGen;
// using static InstallWalls;

// public class CubeMovement : MonoBehaviour
// {
//     public bool EnableW = true;
//     public bool EnableA = true;
//     public bool EnableS = true;
//     public bool EnableD = true;

//     [Header("Cube Settings")]
//     public float cubeSpeed = 0.5f;

//     private Vector3 cubePosition;
//     private float cubeRadius;

//     void Start()
//     {
//         //cubePosition = transform.position;
//         cubeRadius = transform.localScale.x / 2; // Assuming the cube is a perfect cube

//         // int[][] mazeGrid = NewMaze();
//         // Install(mazeGrid);
//         // PrintMaze(mazeGrid);
//         // Retrieve the string from PlayerPrefs
//         string flattenedString = PlayerPrefs.GetString("MazeGrid");

//         // Split the string into individual rows
//         string[] rowStrings = flattenedString.Split(';');

//         // Convert row strings back to integer arrays
//         int[][] mazeGrid = new int[rowStrings.Length][];
//         for (int i = 0; i < rowStrings.Length; i++)
//         {
//             string[] stringValues = rowStrings[i].Split(',');
//             mazeGrid[i] = new int[stringValues.Length];

//             for (int j = 0; j < stringValues.Length; j++)
//             {
//                 mazeGrid[i][j] = int.Parse(stringValues[j]);
//             }
//         }
        
//         Install(mazeGrid);
//         cubePosition = transform.position;
     


//     }

//     void FixedUpdate()
//     {
//         float horizontal = Input.GetAxis("Horizontal");
//         float vertical = Input.GetAxis("Vertical");

//         Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized * cubeSpeed;

//         MoveCube(movement);
//     }

//     void MoveCube(Vector3 movement)
//     {
//         Vector3 newPosition = cubePosition + movement * Time.deltaTime;

//         // Check if the new position is valid (not colliding with the maze)
//         if (!IsCollidingWithMaze(newPosition))
//         {
//             cubePosition = newPosition;
//             transform.position = cubePosition;
//         }
//     }

//     bool IsCollidingWithMaze(Vector3 position)
//     {
//         // Check for collisions with maze walls based on the cube's position and size
//         Collider[] colliders = Physics.OverlapBox(position, Vector3.one * cubeRadius);
//         foreach (Collider collider in colliders)
//         {
//             if (collider.gameObject.CompareTag("maze"))
//             {
//                 return true; // Colliding with a maze wall
//             }
//         }
//         return false; // Not colliding with any maze wall
//     }
// }