using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public new CameraMove camera;
    public bool is_Selected = false;
    private Vector3 mouseWorldPos;
    //public Vector3 position = transform.position;
    public float speedspeedspeed;
    // Start is called before the first frame update
    void Start()
    {
        mouseWorldPos = transform.position;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && is_Selected)
        {
            mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            Debug.Log(mouseWorldPos);
        }

        Debug.Log(mouseWorldPos);
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speedspeedspeed * Time.deltaTime);

    }

    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (is_Selected)
                is_Selected = false;
            //Vector3 mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            //if (Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).x) < 5 && Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).y) < 5)
            //{
            //    GetComponent<EnemyMovement>().is_Selected = true;

            //}
            else if (!is_Selected)
                is_Selected = true;
        }
    }
}
