using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Play()
    {
        // SceneManager.LoadScene(1); // build setting 내의 scene idx
         SceneManager.LoadScene("Level_1");
    }
    public void Shop()
    {
        SceneManager.LoadScene("Tmp_Shop");
    }
    public void Ranking()
    {
        SceneManager.LoadScene("Tmp_Ranking");
    }
    public void goMain()
    {
        SceneManager.LoadScene("TitleMain");
    }
    public void Quit()
    {
        Application.Quit();
    }
    
}
