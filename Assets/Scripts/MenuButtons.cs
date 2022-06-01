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
        GameSettings.Instance.SetGameSize((ushort)size);
        Debug.Log("[INFO] gameSize: " + size);
    }


    public void SetDifficulty(string difficulty)
    {
        GameSettings.Instance.SetGameMode(difficulty);
        Debug.Log("[INFO] gameDifficulty: " + difficulty);
    }


    public void ActivateObject(GameObject obj)
    {
        obj.SetActive(true);
    }


    public void DeactivateObject(GameObject obj)
    {
        obj.SetActive(false);
    }


    public void QuitGame()
    {
        Debug.Log("[INFO] Quit Game");
        Application.Quit();
    }
}
