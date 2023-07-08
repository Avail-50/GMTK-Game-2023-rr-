using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    
    public float speed;
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
            float size = GetComponent<Camera>().orthographicSize;
            //if (distanceChange > 0 && size > 30)
            //{
                //GetComponent<Camera>().orthographicSize -= distanceChange * scrollSpeed;
            //}
            //else if (distanceChange < 0 && size  1)
            //{
                //GetComponent<Camera>().orthographicSize -= distanceChange * scrollSpeed;
            //}
            GetComponent<Camera>().orthographicSize -= distanceChange * scrollSpeed;
            
        }

//        if (Input.GetButtonDown("Fire2"))
//        {
//            Vector3 mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
//            if (Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).x) < 5 && Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).y) < 5)
//            {
//                GetComponent<EnemyMovement>().is_Selected = true;
//
//            }
                
//        }
        // allow the user to lock and unlock cursor from screen
        //if (Input.GetKey(KeyCode.Escape))
            //Cursor.lockState = CursorLockMode.None;
        //else if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonDown(0))
            //Cursor.lockState = CursorLockMode.Locked;

        float upDown = Input.GetAxis("Vertical") * speed;
        float leftRight = Input.GetAxis("Horizontal") * speed;

        upDown *= GetComponent<Camera>().orthographicSize * Time.deltaTime;
        leftRight *= GetComponent<Camera>().orthographicSize * Time.deltaTime;
        
        Vector3 movement = new Vector3(leftRight, upDown, 0);
        movement = Vector3.ClampMagnitude(movement, 1);
        transform.Translate(movement);


    }
}
