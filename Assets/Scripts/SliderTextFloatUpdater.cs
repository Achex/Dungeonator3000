using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderTextFloatUpdater : MonoBehaviour
{
    public Slider slider;
    public TMP_Text textMeshPro;

    private void Start()
    {
        // Update text initially
        UpdateText();
    }

    public void UpdateText()
    {
        string formattedFloat = slider.value.ToString("0.00");
        // Update the TextMeshPro component with the current value of the slider
        textMeshPro.text = formattedFloat;
    }
}