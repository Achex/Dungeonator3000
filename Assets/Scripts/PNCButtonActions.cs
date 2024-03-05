using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PNCButtonActions : MonoBehaviour
{
    public GameObject loadScreen;
    public GameObject insideButton;
    public GameObject townButton;
    public GameObject streetButtonFromTown;
    public GameObject streetButtonFromInside;
    public TMP_Text progressText;

    private readonly string controlnetLineartModel = "control_v11p_sd15_lineart [43d4be0d]";
    private readonly string controlnetLineartRealisticPreprocessor = "lineart_realistic";
    private bool detected = false;

    public void InsidePress()
    {
        print("Inside button press");

        insideButton.SetActive(false);
        townButton.SetActive(false);
        
        PlayerPrefs.SetString("PNCRoom", "Inside");
        
        if (!File.Exists("Assets/Resources/stableDiffusionInside.jpeg"))
        {
            StartCoroutine(CreateRoom("Inside"));
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("stableDiffusionInside");
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        streetButtonFromInside.SetActive(true);
    }

    public void TownPress()
    {
        print("Town button press");

        insideButton.SetActive(false);
        townButton.SetActive(false);

        PlayerPrefs.SetString("PNCRoom", "Town");

        if (!File.Exists("Assets/Resources/stableDiffusionTown.jpeg"))
        {
            StartCoroutine(CreateRoom("Town"));
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("stableDiffusionTown");
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        streetButtonFromTown.SetActive(true);
    }

    public void StreetFromInsidePress()
    {
        print("Street button press");

        streetButtonFromInside.SetActive(false);

        PlayerPrefs.SetString("PNCRoom", "Street");

        if (!File.Exists("Assets/Resources/stableDiffusionStreet.jpeg"))
        {
            StartCoroutine(CreateRoom("Street"));
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("stableDiffusionStreet");
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        insideButton.SetActive(true);
        townButton.SetActive(true);
    }

    public void StreetFromTownPress()
    {
        print("Street button press");

        streetButtonFromTown.SetActive(false);

        PlayerPrefs.SetString("PNCRoom", "Street");

        if (!File.Exists("Assets/Resources/stableDiffusionStreet.jpeg"))
        {
            StartCoroutine(CreateRoom("Street"));
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>("stableDiffusionStreet");
            GetComponent<SpriteRenderer>().sprite = sprite;
        }

        insideButton.SetActive(true);
        townButton.SetActive(true);
    }

    private void SetPNCTexture(string room)
    {
        string PNCImagePath = "Assets/Resources/stableDiffusion" + room + ".jpeg";
        TextureImporter textureImporter = AssetImporter.GetAtPath(PNCImagePath) as TextureImporter;

        if (textureImporter != null)
        {
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;

            AssetDatabase.ImportAsset(PNCImagePath);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("Texture importer not found. Make sure the path is correct.");
        }
    }

    //INSERT THE IMAGE AFTER GETTING GENERATED FOR FIRST TIME?
    // public void Update() 
    // {
    //     if (!detected && PlayerPrefs.GetString("PNCRoom").Equals("Street") && File.Exists("Assets/Resources/stableDiffusionStreet.jpeg"))
    //     {
    //         detected = true;
    //         SetPNCTexture("Street");
    //         Sprite sprite = Resources.Load<Sprite>("stableDiffusionStreet");
    //         if (sprite != null)
    //         {
    //             //set the sprite to the SpriteRenderer component
    //             GetComponent<SpriteRenderer>().sprite = sprite;
    //         }
    //         else
    //         {
    //             AssetDatabase.Refresh();
    //             detected = false;
    //         }
    //     }

    //     if (!detected && PlayerPrefs.GetString("PNCRoom").Equals("Town") && File.Exists("Assets/Resources/stableDiffusionTown.jpeg"))
    //     {
    //         detected = true;
    //         SetPNCTexture("Town");
    //         Sprite sprite = Resources.Load<Sprite>("stableDiffusionTown");
    //         if (sprite != null)
    //         {
    //             //set the sprite to the SpriteRenderer component
    //             GetComponent<SpriteRenderer>().sprite = sprite;
    //         }
    //         else
    //         {
    //             AssetDatabase.Refresh();
    //             detected = false;
    //         }
    //     }

    //     if (!detected && PlayerPrefs.GetString("PNCRoom").Equals("Inside") && File.Exists("Assets/Resources/stableDiffusionInside.jpeg"))
    //     {
    //         detected = true;
    //         SetPNCTexture("Inside");
    //         Sprite sprite = Resources.Load<Sprite>("stableDiffusionInside");
    //         if (sprite != null)
    //         {
    //             //set the sprite to the SpriteRenderer component
    //             GetComponent<SpriteRenderer>().sprite = sprite;
    //         }
    //         else
    //         {
    //             AssetDatabase.Refresh();
    //             detected = false;
    //         }
    //     }
    // }

    void Update()
    {
        if (!detected && File.Exists("Assets/Resources/stableDiffusionStreet.jpeg")) 
        {
            detected = true;
            SetPNCTexture("Street");
            Sprite sprite = Resources.Load<Sprite>("stableDiffusionStreet");

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

    IEnumerator CreateRoom(string room)
    {
        loadScreen.SetActive(true);
        progressText.text = "0%";

        string urlCode = PlayerPrefs.GetString("StableDiffusionLink");
        string inputFile = "Assets/Resources/" + room + "PNCBase.png";
        string outputFile = "Assets/Resources/stableDiffusion" + room + ".jpeg";

        byte[] imageBytes = File.ReadAllBytes(inputFile);
        string encodedImage = Convert.ToBase64String(imageBytes);

        ControlnetRequest controlnetJsonData = new()
        {
            controlnet_module = controlnetLineartRealisticPreprocessor,
            controlnet_input_images = new List<string> {encodedImage},
            controlnet_processor_res = 512,
            controlnet_threshold_a = 64,
            controlnet_threshold_b = 64
        };

        UnityWebRequest controlnetRequest = UnityWebRequest.Post(urlCode + "/controlnet/detect", JsonUtility.ToJson(controlnetJsonData), "application/json");
        yield return controlnetRequest.SendWebRequest();

        string controlnetResponseJson = controlnetRequest.downloadHandler.text;

        print("controlnet response: " + controlnetResponseJson);

        string encodedPreprocessedImage = JsonUtility.FromJson<ImageResponse>(controlnetResponseJson).images[0];

        byte[] decodedBytes = Convert.FromBase64String(encodedPreprocessedImage);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(decodedBytes);

        print("---------NEW ROOM------------");

        string promptSetting = PlayerPrefs.GetString("Prompt");
        string negativePromptSetting = PlayerPrefs.GetString("NegativePrompt");
        string modelSetting = PlayerPrefs.GetString("ModelSetting");
        string samplerSetting = PlayerPrefs.GetString("SamplerSetting");
        float samplingStepsSliderSetting = PlayerPrefs.GetFloat("SamplingStepsSlierSetting");
        float CFGScaleSetting = PlayerPrefs.GetFloat("CFGScaleSetting");
        string upscalerSetting = PlayerPrefs.GetString("UpscalerSetting");
        float denoisingStrengthSetting = PlayerPrefs.GetFloat("DenoisingStrengthSetting");
        float upscaleAmountSlider = PlayerPrefs.GetFloat("UpscaleAmountSlider");

        print(promptSetting);
        print(negativePromptSetting);
        print(modelSetting);
        print(samplerSetting);
        print(samplingStepsSliderSetting);
        print(CFGScaleSetting);
        print(upscalerSetting);
        print(denoisingStrengthSetting);
        print(upscaleAmountSlider);

        print("----------------------------");

        StableDiffusionRequest jsonData = new()
        {
            sd_model_checkpoint = modelSetting,
            prompt = promptSetting,
            negative_prompt = negativePromptSetting,
            batch_size = 1,
            steps = Mathf.RoundToInt(samplingStepsSliderSetting),
            cfg_scale = CFGScaleSetting,
            enable_hr = true,
            hr_scale = upscaleAmountSlider,
            hr_upscaler = upscalerSetting,
            hr_second_pass_steps = 20,
            denoising_strength = denoisingStrengthSetting,
            sampler_index = samplerSetting,
            alwayson_scripts = new AlwaysonScripts()
            {
                controlnet = new ControlnetScript() 
                {
                    args = new List<ControlnetArgs>() 
                    {
                        new ControlnetArgs() 
                        {
                            input_image = encodedPreprocessedImage, //encodedImage bypasses preprocessor, switch to encodedPreprocessedImage to use preprocessor
                            model = controlnetLineartModel,
                            resize_mode = 1,
                            control_mode = 0,
                            weight = 1
                        }   
                    }
                }
            }
        };

        UnityWebRequest request = UnityWebRequest.Post(urlCode + "/sdapi/v1/txt2img", JsonConvert.SerializeObject(jsonData), "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 3600;
        yield return request.SendWebRequest();

        string responseJson = request.downloadHandler.text;

        print("Stable Diffusion response: " + responseJson);

        string imageBase64 = JsonUtility.FromJson<ImageResponse>(responseJson).images[0];

        byte[] imageBytesOutput = Convert.FromBase64String(imageBase64);
        Texture2D texOutput = new Texture2D(2, 2);
        texOutput.LoadImage(imageBytesOutput);

        SaveImage(texOutput, outputFile);

        //print("Done, image saved as " + outputFileName);

        AssetDatabase.Refresh();

        SetPNCTexture(room);

        AssetDatabase.Refresh();

        Sprite sprite = Resources.Load<Sprite>("stableDiffusion" + room);
            
        //set the sprite to the SpriteRenderer component
        GetComponent<SpriteRenderer>().sprite = sprite;

        loadScreen.SetActive(false);

        // int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // SceneManager.LoadScene(currentSceneIndex);
    }

    void SaveImage(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
    }
}
