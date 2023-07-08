using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private float distance;
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().orthographicSize = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float distanceChange = Input.GetAxis("Mouse ScrollWheel");
            GetComponent<Camera>().orthographicSize -= distanceChange * scrollSpeed;
            
        }

        // allow the user to lock and unlock cursor from screen
        //if (Input.GetKey(KeyCode.Escape))
            //Cursor.lockState = CursorLockMode.None;
        //else if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
            //Cursor.lockState = CursorLockMode.Locked;
    }
}
