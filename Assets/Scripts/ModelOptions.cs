using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;
using TMPro;
using System.Collections.Generic; 

public class ModelOptions : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private string url;
    private readonly string endpoint = "/sdapi/v1/sd-models";

    void Start()
    {
        url = PlayerPrefs.GetString("StableDiffusionLink") + endpoint;
        StartCoroutine(GetModelTitles());
    }

    IEnumerator GetModelTitles()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                //Debug.Log(jsonResponse);

                // Parse the JSON response as a JArray
                JArray jsonArray = JArray.Parse(jsonResponse);

                List<string> models = new List<string>();

                foreach (JObject model in jsonArray)
                {
                    string name = (string)model["title"];
                    models.Add(name);
                    //Debug.Log("Name: " + name);
                }

                // Assign the list of names to the dropdown options
                dropdown.ClearOptions();
                dropdown.AddOptions(models);
            }
        }
    }
}