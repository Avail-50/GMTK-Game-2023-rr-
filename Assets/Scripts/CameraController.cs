using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
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
            GetComponent<Camera>().orthographicSize = Mathf.Clamp(size - distanceChange * scrollSpeed, 1, 30);
            //GetComponent<Camera>().orthographicSize -= distanceChange * scrollSpeed;

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

    /*public float springConstant;
    public float snapDistance;
    public float smoothSpeed;

    public Transform target;

    float smoothTime = 0.1f;
    //float yVelocity = 0.0f;

    private Vector3 velocity3 = Vector3.zero;

    private Vector2 velocity = new(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {

        //float smoothTime = Time.deltaTime * smoothSpeed;
        //Vector2 target = new(player.transform.position.x, player.transform.position.y);
        //Vector2 newpos = SmoothDamp(target, smoothTime);
        //transform.position = new(newpos.x, newpos.y, transform.position.z);

        //float newPosition = Mathf.SmoothDamp(transform.position.y, target.position.y, ref yVelocity, smoothTime);
        //transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);

        Vector3 targetPosition = target.TransformPoint(new Vector3(0, 0, transform.position.z));

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity3, smoothTime);

    }

    
    
    
    
    
    private Vector2 SmoothDamp(Vector2 target, float smoothTime)
    {
        Vector2 pos = new(transform.position.x, transform.position.y);
        if (smoothTime >= 1) return target;
        Vector2 distance = target - pos;
        if (Mathf.Abs(distance.x) < snapDistance &&
            Mathf.Abs(distance.y) < snapDistance) return target;
        Vector2 springForce = distance * springConstant;
        Vector2 dampingForce = (float)Mathf.Sqrt(springConstant) * -2 * velocity;
        Vector2 force = springForce + dampingForce;
        velocity += force * smoothTime;
        Vector2 displacement = velocity * smoothTime;
        if (displacement.sqrMagnitude > distance.sqrMagnitude) return target;
        return pos + displacement;
    }*/
}
