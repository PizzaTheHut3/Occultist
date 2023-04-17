using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    public InputField sensitivityBox;
    public TMP_Text Level1Text;
    public TMP_Text Level2Text;
    public TMP_Text Level3Text;
    public TMP_Text TitleText;

    public void Start()
    {
        sensitivityBox.GetComponent<UnityEngine.UI.InputField>().text = PlayerPrefs.GetFloat("MouseSense", 200).ToString();
        if (PlayerPrefs.GetFloat("Level1", 0) != 0)
        {
            Level1Text.text = PlayerPrefs.GetFloat("Level1", 0).ToString("f2") + "s";
        }
        if (PlayerPrefs.GetFloat("Level2", 0) != 0)
        {
            Level2Text.text = PlayerPrefs.GetFloat("Level2", 0).ToString("f2") + "s";
        }
        if (PlayerPrefs.GetFloat("Level3", 0) != 0)
        {
            Level3Text.text = PlayerPrefs.GetFloat("Level3", 0).ToString("f2") + "s";
        }
    }

    public void Update()
    {
        //scales title text back and forth to make it look cool
        TitleText.transform.localScale = Vector3.Lerp(new Vector3(2, 2, 2), new Vector3(2.4f, 2.4f, 2.4f), .1f * Mathf.PingPong(Time.time, 1));
    }

    public void PlayGame()
    {
        Application.LoadLevel("Tutorial");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        Application.LoadLevel(PlayerPrefs.GetString("LastLevel", "Tutorial"));
    }

    public void setSense()
    {
        PlayerPrefs.SetFloat("MouseSense", float.Parse(sensitivityBox.GetComponent<UnityEngine.UI.InputField>().text));
    }
}
