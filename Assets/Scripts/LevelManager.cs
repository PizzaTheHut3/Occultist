using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false;
    public Text gameText;
    public Text enemiesKilledText;
    public string nextLevel = "";
    public AudioClip winSFX;
    int enemiesKilled = 0;
    public Text timerText;
    public float timer = 0f;
    public GameObject UI;
    private bool isLevelBeat = false;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameText.text = "GameOver!";
        enemiesKilledText.text = "0 SLAIN";
        if (PlayerPrefs.GetInt("ShowHUD", 1) == 0)
        {
            UI.SetActive(false);
        }
        else
        {
            UI.SetActive(true);
        }
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            gameText.gameObject.SetActive(true);
        }
        enemiesKilledText.text = string.Format("{0} SLAIN", enemiesKilled);
        if (!isGameOver)
        {
            timer += Time.deltaTime;
            timerText.text = string.Format("{0:0.00}", timer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelBeat();
        }
    }

    public void LevelBeat()
    {
        isGameOver = true;
        isLevelBeat = true;
        gameText.text = "Level Complete!";
        if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name, 999) > timer)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, timer);
        }

        AudioSource.PlayClipAtPoint(winSFX, transform.position);
        if (nextLevel != "Level4")
        { //replace "Level3" with the name of the final level in the game.
            Invoke("LoadLevel", 2f);
        }
    }

    public void LevelLost()
    {
        if (isLevelBeat) return;
        isGameOver = true;
        gameText.gameObject.SetActive(true);
        gameText.text = "GAME OVER!";
        Invoke("LoadCurrentLevel", 2);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnemyKilled()
    {
        enemiesKilled += 1;
    }
}
