using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void SetSize(System.Single size)
    {
        SudokuData.Instance.SetSize((ushort)size);
        Debug.Log("[INFO] gameSize: " + size);
    }


    public void SetDifficulty(string difficulty)
    {
        SudokuData.Instance.SetDifficulty(difficulty);
        Debug.Log("[INFO] gameDifficulty: " + difficulty);
    }
}
