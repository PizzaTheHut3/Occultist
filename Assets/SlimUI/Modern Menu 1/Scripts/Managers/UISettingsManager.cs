using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu
{
    public class UISettingsManager : MonoBehaviour
    {
        [Header("GAME SETTINGS")]
        public GameObject showhudtext;

        // sliders
        public GameObject musicSlider;

        private float sliderValue = 0.0f;
        private float sliderValueXSensitivity = 0.0f;
        private float sliderValueYSensitivity = 0.0f;
        private float sliderValueSmoothing = 0.0f;


        public void Start()
        {

            // check slider values
            musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume");

            // check hud value
            if (PlayerPrefs.GetInt("ShowHUD") == 0)
            {
                showhudtext.GetComponent<TMP_Text>().text = "off";
            }
            else
            {
                showhudtext.GetComponent<TMP_Text>().text = "on";
            }
        }

        public void Update()
        {
            //sliderValue = musicSlider.GetComponent<Slider>().value;
            //sliderValueXSensitivity = sensitivityXSlider.GetComponent<Slider>().value;
            //sliderValueYSensitivity = sensitivityYSlider.GetComponent<Slider>().value;
            ///sliderValueSmoothing = mouseSmoothSlider.GetComponent<Slider>().value;
        }

        public void MusicSlider()
        {
            //PlayerPrefs.SetFloat("MusicVolume", sliderValue);
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.GetComponent<Slider>().value);
            PlayerPrefs.SetFloat("Volume", musicSlider.GetComponent<Slider>().value);
        }

        // the playerprefs variable that is checked to enable hud while in game
        public void ShowHUD()
        {
            if (PlayerPrefs.GetInt("ShowHUD") == 0)
            {
                PlayerPrefs.SetInt("ShowHUD", 1);
                showhudtext.GetComponent<TMP_Text>().text = "on";
            }
            else if (PlayerPrefs.GetInt("ShowHUD") == 1)
            {
                PlayerPrefs.SetInt("ShowHUD", 0);
                showhudtext.GetComponent<TMP_Text>().text = "off";
            }
        }
    }
}