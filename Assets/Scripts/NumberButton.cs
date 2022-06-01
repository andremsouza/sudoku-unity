using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumberButton : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{
    public ushort Value = 0;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("[INFO] NumberButton.OnPointerClick()");
        GameEvents.UpdateSquareNumberMethod(Value);
    }


    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("[INFO] NumberButton.OnSubmit()");
    }
}
