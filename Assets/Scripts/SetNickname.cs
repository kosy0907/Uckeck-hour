using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNickname : MonoBehaviour
{
    public InputField inputNickname;
    public void Save()
    {
        PlayerPrefs.SetString("Nickname", inputNickname.text);
    }
}
