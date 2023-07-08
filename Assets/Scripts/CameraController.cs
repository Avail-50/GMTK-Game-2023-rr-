using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public float springConstant;
    public float snapDistance;
    public float smoothSpeed;
    public GameObject player;

    private Vector2 velocity = new(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float smoothTime = Time.deltaTime * smoothSpeed;
        Vector2 target = new(player.transform.position.x, player.transform.position.y);
        Vector2 newpos = SmoothDamp(target, smoothTime);
        transform.position = new(newpos.x, newpos.y, transform.position.z);
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
    }
}
