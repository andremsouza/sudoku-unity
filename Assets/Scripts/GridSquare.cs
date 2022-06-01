using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public GameObject NumberText;
    private int _number = 0;
    private int _correctNumber;
    private bool _isSelected = false;
    private int _squareIndex = -1;
    private bool hasDefaultValue = false;


    public bool IsCorrectNumberSet() { return _number == _correctNumber; }


    public void SetHasDefaultValue(bool value) { hasDefaultValue = value; }


    public bool GetHasDefaultValue() { return hasDefaultValue; }


    public bool IsSelected() { return _isSelected; }


    public void SetSquareIndex(int index) { _squareIndex = index; }


    public void SetCorrectNumber(int number) { _correctNumber = number; }


    public void SetCorrectNumber()
    {
        _number = _correctNumber;
        DisplayText();
    }


    public int GetCorrectNumber() { return _correctNumber; }


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        if (NumberText.GetComponent<TextMeshProUGUI>() == null)
            Debug.LogError("NumberText must have a TextMeshProUGUI script attached to it");
        _isSelected = false;
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


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[INFO] GridSquare.OnPointerClick()");
        _isSelected = true;
        GameEvents.SquareSelectedMethod(_squareIndex);
    }


    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("[INFO] GridSquare.OnSubmit()");
    }


    private void OnEnable()
    {
        GameEvents.OnUpdateSquareNumber += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
    }


    private void OnDisable()
    {
        GameEvents.OnUpdateSquareNumber -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
    }


    public void OnSetNumber(int number)
    {
        if (_isSelected && !hasDefaultValue)
        {
            SetNumber(number);
            if (_number != _correctNumber && _number != 0)
            {
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongNumberMethod();
            }
            else
            {
                hasDefaultValue = true;
                var colors = this.colors;
                colors.normalColor = Color.white;
                this.colors = colors;
            }
        }
    }


    public void OnSquareSelected(int squareIndex)
    {
        if (_squareIndex == squareIndex)
            _isSelected = true;
        else
            _isSelected = false;
    }
}
