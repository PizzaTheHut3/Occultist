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

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameText.text = "GameOver!";
        enemiesKilledText.text = "0 SLAIN";
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver){
            gameText.gameObject.SetActive(true);
        }
        enemiesKilledText.text = string.Format("{0} SLAIN", enemiesKilled);
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            LevelBeat();
        }
    }

    public void LevelBeat(){
        isGameOver = true;
        gameText.text = "Level Complete!";
        
        AudioSource.PlayClipAtPoint(winSFX, transform.position);
        if (SceneManager.GetActiveScene().name != "Level3"){ //replace "Level3" with the name of the final level in the game.
            Invoke("LoadLevel", 2f);
        }
    }

    public void LevelLost() {
        isGameOver = true;
        gameText.gameObject.SetActive(true);
        gameText.text = "GAME OVER!";
        Invoke("LoadCurrentLevel", 2);
    }

    void LoadLevel(){
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnemyKilled() {
        enemiesKilled += 1;
    }
}
