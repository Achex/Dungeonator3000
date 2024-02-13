using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using System.IO;
using Newtonsoft.Json;
using TMPro;

public class StableDiffusionCaller : MonoBehaviour
{
    //public TMP_InputField promptInput;

    //private string urlCode = "https://f8d04dbda3c37903cc.gradio.live"; //Miss the last /
    private string urlCode;
    //private string prompt = "stone dungeon with lots of variety in detail, different art styles, exciting";
    private string prompt;
    private string negativePrompt = "blurry, boring, bland, colourless";
    private string controlnetMaze = "Assets/Resources/mazeImage.png";
    private string controlnetPreprocessor = "lineart_standard (from white bg & black line)";
    private string controlnetModel = "control_v11p_sd15_seg_fp16 [ab613144]";
    private string outputFileName = "Assets/Resources/stableDiffusionMaze.jpeg";
    private string stableDiffusionModel = "photosomnia_omega.safetensors [8fddb78c8d]";
    private string upscalar = "R-ESRGAN 4x+"; //i changed this from ESRGAN_4x
    private float denoisingStrength = 0.7f;

    public void Start() 
    {
        urlCode = PlayerPrefs.GetString("StableDiffusionLink");
    }

    IEnumerator ProcessImages()
    {
        //encode image to base64 for json request
        byte[] imageBytes = File.ReadAllBytes(controlnetMaze);
        string encodedImage = Convert.ToBase64String(imageBytes);

        ControlnetRequest controlnetJsonData = new()
        {
            controlnet_module = controlnetPreprocessor,
            controlnet_input_images = new List<string> {encodedImage},
            controlnet_processor_res = 512,
            controlnet_threshold_a = 64,
            controlnet_threshold_b = 64
        };

        UnityWebRequest controlnetRequest = UnityWebRequest.Post(urlCode + "/controlnet/detect", JsonUtility.ToJson(controlnetJsonData), "application/json");
        yield return controlnetRequest.SendWebRequest();

        string controlnetResponseJson = controlnetRequest.downloadHandler.text;

        string encodedPreprocessedImage = JsonUtility.FromJson<ImageResponse>(controlnetResponseJson).images[0];

        byte[] decodedBytes = Convert.FromBase64String(encodedPreprocessedImage);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(decodedBytes);
        
        StableDiffusionRequest jsonData = new()
        {
            sd_model_checkpoint = stableDiffusionModel,
            prompt = prompt,
            negative_prompt = negativePrompt,
            batch_size = 1,
            steps = 20,
            cfg_scale = 5,
            enable_hr = true,
            hr_resize_x = 1024,
            hr_resize_y = 1024,
            hr_scale = 2,
            hr_upscaler = upscalar,
            hr_second_pass_steps = 20,
            denoising_strength = denoisingStrength,
            alwayson_scripts = new AlwaysonScripts()
            {
                controlnet = new ControlnetScript() 
                {
                    args = new List<ControlnetArgs>() 
                    {
                        new ControlnetArgs() 
                        {
                            input_image = encodedImage, //encodedImage bypasses preprocessor, switch to encodedPreprocessedImage to use preprocessor
                            model = controlnetModel,
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
        yield return request.SendWebRequest();

        string responseJson = request.downloadHandler.text;

        string imageBase64 = JsonUtility.FromJson<ImageResponse>(responseJson).images[0];

        byte[] imageBytesOutput = Convert.FromBase64String(imageBase64);
        Texture2D texOutput = new Texture2D(2, 2);
        texOutput.LoadImage(imageBytesOutput);

        SaveImage(texOutput, outputFileName);

        Debug.Log("Done, image saved as " + outputFileName);
    }

    void SaveImage(Texture2D texture, string fileName)
    {
        byte[] bytes = texture.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
    }
}
