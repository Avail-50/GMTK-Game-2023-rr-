using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private EnemyMovement onCollect;
    private int buff = 1;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D obj)
    {
        onCollect = obj.gameObject.GetComponent<EnemyMovement>();

        onCollect.OnCollect(buff);
        Destroy(gameObject);

    }
}
