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
         SceneManager.LoadScene("InGameScene_lvl1");
    }
    public void Shop()
    {
        SceneManager.LoadScene("StoreScene");
    }
    public void Ranking()
    {
        SceneManager.LoadScene("RankingScene");
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
