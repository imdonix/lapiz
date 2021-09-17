using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] public Text text;

    public void UpdateStatu(float amount, float of)
    {
        float prec = amount / of;
        slider.value = prec;
        text.text = string.Format("{0} / {1}", Mathf.RoundToInt(of), Mathf.RoundToInt(amount));
    }
}
