using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;

    public TextMeshProUGUI healthText;

    private void Awake()
    {
        instance = this;
    }
}


