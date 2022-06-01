using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public List<GameObject> ErrorImages;
    public GameObject GameOverPopup;
    public GameObject MainMenuButton;
    public GameObject ExitButton;
    public GameObject SudokuSolveButton;

    private int _lives = 0, _errorNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        _lives = ErrorImages.Count;
        _errorNumber = 0;
    }


    private void WrongNumber()
    {
        if (_errorNumber < ErrorImages.Count)
        {
            ErrorImages[_errorNumber++].SetActive(true);
            _lives--;
        }

        CheckGameOver();
    }


    private void CheckGameOver()
    {
        if (_lives <= 0)
        {
            GameEvents.OnGameOverMethod();
            GameOverPopup.SetActive(true);
            MainMenuButton.SetActive(false);
            ExitButton.SetActive(false);
            SudokuSolveButton.SetActive(false);

        }
    }


    private void OnEnable()
    {
        GameEvents.OnWrongNumber += WrongNumber;
    }


    private void OnDisable()
    {
        GameEvents.OnWrongNumber -= WrongNumber;
    }
}
