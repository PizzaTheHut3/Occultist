using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver = false;
    public Text gameText;
    public string nextLevel = "";
    public AudioClip winSFX;

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        gameText.text = "GameOver!";
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver){
            gameText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            LevelBeat();
        }
    }

    void LevelBeat(){
        isGameOver = true;
        gameText.text = "Level Complete!";
        
        AudioSource.PlayClipAtPoint(winSFX, transform.position);
        if (SceneManager.GetActiveScene().name != "Level3"){ //replace "Level3" with the name of the final level in the game.
            Invoke("LoadLevel", 2f);
        }
    }

    void LoadLevel(){
        SceneManager.LoadScene(nextLevel);
    }
}
