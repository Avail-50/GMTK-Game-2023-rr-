using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Vector3 mouseWorldPos;
    public float speedspeedspeed;
    // Start is called before the first frame update
    void Start()
    {
        mouseWorldPos = transform.position;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            Debug.Log(mouseWorldPos);
        }

        Debug.Log(mouseWorldPos);
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speedspeedspeed * Time.deltaTime);

    }
}
