using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject GameOverPopup;
    public GameObject MainMenuButton;
    public GameObject ExitButton;
    public GameObject SudokuSolveButton;

    public TMP_Text textClock;

    // Start is called before the first frame update
    void Start()
    {
        GameOverPopup.SetActive(false);
        textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }


    private void OnBoardCompleted()
    {
        GameOverPopup.SetActive(true);
        MainMenuButton.SetActive(false);
        ExitButton.SetActive(false);
        SudokuSolveButton.SetActive(false);
        textClock.text = Clock.Instance.GetCurrentTimeText().text;
    }

    private void OnEnable()
    {
        GameEvents.OnBoardCompleted += OnBoardCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnBoardCompleted -= OnBoardCompleted;
    }
}
