using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    [SerializeField] FloatingHealth healthBar;
    private BoxCollider2D aim;
    public Transform enemy;
    public float counter = 0;
    public float force;
    private Rigidbody2D rb2D;
    private EnemyController dealDamage;
    //[SerializeField] Camera camera;
    //[SerializeField] Transform hero;


    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealth>();
        dealDamage = enemy.GetComponent<EnemyController>();
        rb2D = enemy.GetComponent<Rigidbody2D>();
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

    void OnAttack()
    {
        Debug.Log("Attack");

        //adds force to rigidbodies
        Vector3 direction = enemy.transform.position - transform.position;
        direction.Normalize();

        rb2D.AddForce(direction * force);

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
