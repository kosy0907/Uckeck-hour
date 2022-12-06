using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetNickname : MonoBehaviour
{
    public InputField inputNickname;
    public void Save()
    {
        PlayerPrefs.SetString("Nickname", inputNickname.text);
        SceneManager.LoadScene(0);
    }
}
