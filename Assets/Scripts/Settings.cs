using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private BoardGenerator boardGenerator;
    [SerializeField] private Slider rowSlider, columnSlider, bombSlider;

    private void OnEnable()
    {
        rowSlider.value = boardGenerator.Rows;
        columnSlider.value = boardGenerator.Columns;
        bombSlider.value = boardGenerator.Bombs;
    }

    private void OnDisable()
    {
        boardGenerator.Rows = (int)rowSlider.value;
        boardGenerator.Columns = (int)columnSlider.value;
        boardGenerator.Bombs = (int)bombSlider.value;
    }

    public void UpdateBombLimits()
    {
        float x = rowSlider.value;
        float y = columnSlider.value;

        int bombMax = (int)(((x * y) / 10) * 8);

        bombSlider.maxValue = bombMax;
    }
}
