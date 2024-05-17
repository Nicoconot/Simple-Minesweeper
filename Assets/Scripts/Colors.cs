using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Colors : MonoBehaviour
{
    public static Color[] NumberColors = new Color[8];

    [SerializeField] private Color[] numberColor = new Color[8];

    private void Awake()
    {
        for (int i = 0; i < numberColor.Length; i++)
        {
            NumberColors[i] = numberColor[i];
        }
    }
}
