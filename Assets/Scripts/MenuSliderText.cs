using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuSliderText : MonoBehaviour
{

    public GameObject SliderTextObject;
    // Start is called before the first frame update
    void Start()
    {
        if (SliderTextObject.GetComponent<TMP_Text>() == null)
            Debug.LogError("SliderTextObject must have a SliderTextObject script attached to it");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSliderValueChanged(System.Single value)
    {
        var sliderText = GetComponent<TMP_Text>();
        sliderText.text = value.ToString("0");
    }
}
