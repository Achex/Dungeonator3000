using UnityEngine;

public static class InstallWalls
{
    public static void Install(int[][] mazeGrid) 
    {
        for(int i=0; i<mazeGrid.Length; i++) 
        {
            for(int j=0; j<mazeGrid[i].Length; j++) 
            {
                if (mazeGrid[i][j] == 1) 
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.name = "Wall "+i.ToString()+" "+j.ToString();
                    cube.transform.position = new Vector3(j-7, 0, i-7);
                    cube.transform.localScale = new Vector3(1,1,1);
                    cube.tag ="maze";
                    Material testMaterial = Resources.Load<Material>("testMat1");
                    if (testMaterial != null)
                    {
                        cube.GetComponent<Renderer>().material = testMaterial;
                    }
                    else
                    {
                        Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
                    }
                    Rigidbody cubeRigidbody = cube.AddComponent<Rigidbody>();

                    cubeRigidbody.useGravity = false;

                    cubeRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    cubeRigidbody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

                    BoxCollider boxCollider = cube.AddComponent<BoxCollider>();
                }
            }
        }


        GameObject leftCap = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftCap.name = "Left Cap";
        leftCap.transform.position = new Vector3(-8, 0, 0);
        leftCap.transform.localScale = new Vector3(1,1,1);
        leftCap.tag ="maze";

        Material testMaterialLC = Resources.Load<Material>("testMat1");
        if (testMaterialLC != null)
        {
            leftCap.GetComponent<Renderer>().material = testMaterialLC;
        }
        else
        {
            Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
        }
        Rigidbody cubeRigidbodyLC = leftCap.AddComponent<Rigidbody>();

        cubeRigidbodyLC.useGravity = false;

        cubeRigidbodyLC.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        cubeRigidbodyLC.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        BoxCollider boxColliderLC = leftCap.AddComponent<BoxCollider>();


        GameObject rightCap = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightCap.name = "Right Cap";
        rightCap.transform.position = new Vector3(8, 0, 0);
        rightCap.transform.localScale = new Vector3(1,1,1);
        rightCap.tag ="maze";

        Material testMaterialRC = Resources.Load<Material>("testMat1");
        if (testMaterialRC != null)
        {
            rightCap.GetComponent<Renderer>().material = testMaterialRC;
        }
        else
        {
            Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
        }
        Rigidbody cubeRigidbodyRC = rightCap.AddComponent<Rigidbody>();

        cubeRigidbodyRC.useGravity = false;

        cubeRigidbodyRC.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        cubeRigidbodyRC.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        BoxCollider boxColliderRC = rightCap.AddComponent<BoxCollider>();


        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
        goal.name = "Goal";
        goal.transform.position = new Vector3(7, 0, 0);
        goal.transform.localScale = new Vector3(1,1,1);
        goal.tag ="goal";

        Material goalMat = Resources.Load<Material>("testMat1");
        if (goalMat != null)
        {
            goal.GetComponent<Renderer>().material = goalMat;
        }
        else
        {
            Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
        }
        Rigidbody goalRigidBody = goal.AddComponent<Rigidbody>();

        goalRigidBody.useGravity = false;

        goalRigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        goalRigidBody.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

        BoxCollider goalBoxCollider = goal.AddComponent<BoxCollider>();

        GoalScript goalScript = goal.AddComponent<GoalScript>();
    }
}