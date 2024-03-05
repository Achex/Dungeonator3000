using UnityEngine;
using UnityEditor;
using System.IO;

public class MazeInserter : MonoBehaviour
{
    private bool detected = false;

    public string imagePath = "mazeImage";

    static void SetTextureType()
    {
        string path = "Assets/Resources/mazeImage.png";
        TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;

            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("Texture importer not found. Make sure the path is correct.");
        }
    }

    // static void SetPNCTexture(string PNCImagePath)
    // {
    //     TextureImporter textureImporter = AssetImporter.GetAtPath(PNCImagePath) as TextureImporter;

    //     if (textureImporter != null)
    //     {
    //         textureImporter.textureType = TextureImporterType.Sprite;
    //         textureImporter.spriteImportMode = SpriteImportMode.Single;

    //         AssetDatabase.ImportAsset(PNCImagePath);
    //         AssetDatabase.Refresh();
    //     }
    //     else
    //     {
    //         Debug.LogError("Texture importer not found. Make sure the path is correct.");
    //     }
    // }

    void Update()
    {
        if (!detected && File.Exists("Assets/Resources/mazeImage.png")) 
        {
            detected = true;
            SetTextureType();
            Sprite sprite = Resources.Load<Sprite>(imagePath);

            if (sprite != null)
            {
                //set the sprite to the SpriteRenderer component
                GetComponent<SpriteRenderer>().sprite = sprite;
            }
            else
            {
                AssetDatabase.Refresh();
                detected = false;
            }
        }
    }
}
