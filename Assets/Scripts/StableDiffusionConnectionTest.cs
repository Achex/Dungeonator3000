using System.Collections;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class StableDiffusionConnectionTest : MonoBehaviour
{
    public TMP_InputField urlInput;
    public TMP_Text errorMessage;
    public RectTransform thisPage;
    public RectTransform promptPage;

    public void ConfirmURL() 
    {
        errorMessage.gameObject.SetActive(false);
        StartCoroutine(TestStableDiffusionConnection());
    }

    private void displayErrorMessage() 
    {
        errorMessage.gameObject.SetActive(true);
    }

    IEnumerator TestStableDiffusionConnection() 
    {
        string urlInputFormatted = urlInput.text;
        if (urlInput.text.EndsWith("/")) 
        {
            urlInputFormatted = urlInputFormatted[..^1];
        }

        using (UnityWebRequest request = UnityWebRequest.Get(urlInputFormatted + "/internal/ping")) 
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success || request.responseCode != 200)
            {
                Debug.Log(request.error);
                displayErrorMessage();
            }
            else 
            {
                PlayerPrefs.SetString("StableDiffusionLink", urlInputFormatted);

                UnityWebRequest samplersRequest = UnityWebRequest.Get(urlInputFormatted + "/sdapi/v1/samplers");
                UnityWebRequest upscalersRequest = UnityWebRequest.Get(urlInputFormatted + "/sdapi/v1/upscalers");
                UnityWebRequest modelsRequest = UnityWebRequest.Get(urlInputFormatted + "/sdapi/v1/sd-models");

                yield return samplersRequest.SendWebRequest();
                yield return upscalersRequest.SendWebRequest();
                yield return modelsRequest.SendWebRequest();

                if (samplersRequest.result == UnityWebRequest.Result.Success && upscalersRequest.result == UnityWebRequest.Result.Success && modelsRequest.result == UnityWebRequest.Result.Success)
                {
                    JArray jsonSamplerArray = JArray.Parse(samplersRequest.downloadHandler.text);
                    PlayerPrefs.SetString("DefaultSampler", (string)jsonSamplerArray[0]["name"]);

                    JArray jsonUpscalerArray = JArray.Parse(upscalersRequest.downloadHandler.text);
                    PlayerPrefs.SetString("DefaultUpscaler", (string)jsonUpscalerArray[0]["name"]);

                    JArray jsonModelArray = JArray.Parse(modelsRequest.downloadHandler.text);
                    PlayerPrefs.SetString("DefaultModel", (string)jsonModelArray[0]["title"]);
                    
                    PlayerPrefs.Save();
                    thisPage.gameObject.SetActive(false);
                    promptPage.gameObject.SetActive(true);
                }
                else
                {
                    displayErrorMessage();
                }
            }
        }
    }
}
