using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GridSquare : Selectable
{
    public GameObject NumberText;
    private int _number = 0;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (NumberText.GetComponent<TextMeshProUGUI>() == null)
            Debug.LogError("numberText must have a TextMeshProUGUI script attached to it");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayText()
    {
        if (_number <= 0)
            NumberText.GetComponent<TMP_Text>().text = " ";
        else
        {
            NumberText.GetComponent<TMP_Text>().text = _number.ToString();
        }
    }

    public void SetNumber(int number)
    {
        _number = number;
        DisplayText();
    }
}
