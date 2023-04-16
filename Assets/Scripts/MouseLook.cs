using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    Transform playerBody;
    public float mouseSensitivity = 200;
    private float finalMultiplier = 1.0f;
    public float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        finalMouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 200);
    }

    // Update is called once per frame
    void Update()
    {
        finalMultiplier = 1.0f / Time.timeScale;
        float moveX = Input.GetAxis("Mouse X") * finalMultiplier * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * finalMultiplier * mouseSensitivity * Time.deltaTime;

        //yaw
        playerBody.Rotate(Vector3.up * moveX);

        //pitch
        pitch -= moveY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
