using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class SliderVisualizer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;

    public void UpdateNumber(float value)
    {
        numberText.text = value.ToString();
    }
}
