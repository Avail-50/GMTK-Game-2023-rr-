using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    [SerializeField] FloatingHealth healthBar;
    public BoxCollider2D aim;
    public float counter = 0;
    public float attackForce;
    //[SerializeField] Camera camera;
    //[SerializeField] Transform hero;


    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealth>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //aim.transform.rotation = enemy;
        //Vector3 newDirection = Vector3.RotateTowards(transform.forward, enemy.transform.position, singleStep, 0.0f);

        if (counter > 0)
            counter = Mathf.Clamp(counter - Time.deltaTime, 0f, float.PositiveInfinity);
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.transform.position + new(0, 75f, 0);
    }

    

        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        EnemyController closestEnemy = enemies.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).First();
        if (closestEnemy == null) return;

        //adds force to rigidbodies
        Vector3 direction = closestEnemy.transform.position - transform.position;
        direction.Normalize();

        closestEnemy.GetComponent<Rigidbody2D>().AddForce(direction * attackForce);

    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (counter == 0)
        {
            OnAttack();
            counter = 2f;
        }



    }

    public void OnDamaged(int damageTaken)
    {
        health -= damageTaken;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Hero Died");
    }

}
