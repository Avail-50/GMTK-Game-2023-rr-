using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform bigCharacter;
    public int force;
    private Rigidbody2D rb2D;

    //public CameraMove camera;
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

        //Debug.Log(mouseWorldPos);
        transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speedspeedspeed * Time.deltaTime);
        //AddForce(Player.transform.forward)

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

    void OnTriggerStay()
    {
        attack();
    }

    void attack()
    {
        Debug.Log("Attack");

        //adds force to rigidbodies
        Vector3 direction = bigCharacter.transform.position - transform.position;
        direction.Normalize();
        rb2D.AddForce(direction * force);
    }

}
