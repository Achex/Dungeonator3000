using System.Runtime.InteropServices;
using UnityEngine;

public static class InstallWalls
{
    public static void Install(int[][] mazeGrid, string gameMode) 
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


        if (gameMode.Equals("Maze")) 
        {
            GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            goal.name = "Goal";
            goal.transform.position = new Vector3(7.5f, 0, 0);
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

        if (gameMode.Equals("Platformer"))
        {
            for (int i=0; i<mazeGrid.Length; i++)
            {
                GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube1.name = "Wall Top"+i.ToString();
                cube1.transform.position = new Vector3(i-7, 0, 8);
                cube1.transform.localScale = new Vector3(1,1,1);
                cube1.tag ="maze";
                Material testMaterial1 = Resources.Load<Material>("testMat1");
                if (testMaterial1 != null)
                {
                    cube1.GetComponent<Renderer>().material = testMaterial1;
                }
                else
                {
                    Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
                }
                Rigidbody cubeRigidbody1 = cube1.AddComponent<Rigidbody>();

                cubeRigidbody1.useGravity = false;

                cubeRigidbody1.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                cubeRigidbody1.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

                BoxCollider boxCollider1 = cube1.AddComponent<BoxCollider>();


                GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube2.name = "Wall Left"+i.ToString();
                cube2.transform.position = new Vector3(-8, 0, i-7);
                cube2.transform.localScale = new Vector3(1,1,1);
                cube2.tag ="maze";
                Material testMaterial2 = Resources.Load<Material>("testMat1");
                if (testMaterial2 != null)
                {
                    cube2.GetComponent<Renderer>().material = testMaterial2;
                }
                else
                {
                    Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
                }
                Rigidbody cubeRigidbody2 = cube2.AddComponent<Rigidbody>();

                cubeRigidbody2.useGravity = false;

                cubeRigidbody2.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                cubeRigidbody2.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

                BoxCollider boxCollider2 = cube2.AddComponent<BoxCollider>();


                GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube3.name = "Wall Right"+i.ToString();
                cube3.transform.position = new Vector3(8, 0, i-7);
                cube3.transform.localScale = new Vector3(1,1,1);
                cube3.tag ="maze";
                Material testMaterial3 = Resources.Load<Material>("testMat1");
                if (testMaterial3 != null)
                {
                    cube3.GetComponent<Renderer>().material = testMaterial3;
                }
                else
                {
                    Debug.LogError("Material 'testMat1' not found. Make sure the material is in a 'Resources' folder.");
                }
                Rigidbody cubeRigidbody3 = cube3.AddComponent<Rigidbody>();

                cubeRigidbody3.useGravity = false;

                cubeRigidbody3.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                cubeRigidbody3.constraints |= RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;

                BoxCollider boxCollider3 = cube3.AddComponent<BoxCollider>();
            }
        }
    }
}