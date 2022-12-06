using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class wait : MonoBehaviour
{
    public float wait_time = 5f;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() 
    {
        yield return new WaitForSeconds(wait_time);
        Debug.Log(PlayerPrefs.HasKey("Nickname"));
        if(PlayerPrefs.HasKey("Nickname")){
            SceneManager.LoadScene(0);
        }
        else{
            SceneManager.LoadScene(1);
        }
    }
}
