using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerPlatforms : MonoBehaviour
{

    public int platformType;
    public int timeToSwitch = 4;
    public int numberOfPlatformTypes = 3;
    public Color fullColor;
    public Color dropColor;

    private float timeRemaining;
    private int currentOffPlatform = 0;
    private Material mat;
    private BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 0;
        mat = this.GetComponent<Renderer>().material;
        collider = GetComponent<BoxCollider>();
    }


    void Update() {
        timeRemaining -= Time.deltaTime;
        if(timeRemaining < 0) {
            timeRemaining = timeToSwitch;
            currentOffPlatform++;
            if(currentOffPlatform >= numberOfPlatformTypes) {
                currentOffPlatform = 0;
            }
            if(currentOffPlatform == platformType) {
                collider.enabled = false;
                Color oldColor = mat.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0.5f);
                mat.SetColor("_Color", newColor);
            }
            else {
                collider.enabled = true;
                Color oldColor = mat.color;
                Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
                mat.SetColor("_Color", newColor);
            }
        }
    }
}
