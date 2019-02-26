using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseRotation : MonoBehaviour
{
    public float xMultiplier = 100.0f;
    public float yMultiplier = 100.0f;
    private float cameraX = 0.0f;
    private float cameraY = 180.0f;
    CursorLockMode wantedMode;

    // Start is called before the first frame update
    void Start()
    {
        wantedMode = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;

        cameraY = cameraY + Time.deltaTime * xMultiplier * Input.GetAxis("Mouse X");
        cameraX = cameraX + Time.deltaTime * yMultiplier * Input.GetAxis("Mouse Y");
        transform.localEulerAngles = new Vector3(-cameraX, cameraY, 0);
        //transform.Rotate(Vector3.up * Time.deltaTime * xMultiplier * Input.GetAxis("Mouse X"));
        // transform.Rotate(Vector3.left * cameraX);

        SetCursorState();

    }

    // Apply requested cursor state
    void SetCursorState()
    {
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
    }

}