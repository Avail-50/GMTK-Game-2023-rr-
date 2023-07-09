using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public HeroController bigCharacter;
    private Rigidbody2D heroRb2D;
    private NavMeshAgent navMeshAgent;
    private FloatingHealth healthBar;

    public int attackDamage;
    public float attackForce;
    public int maxHealth;
    public float speedspeedspeed;

    [SerializeField] int health;
    public bool isSelected = false;

    private Vector3 mouseWorldPos;
    private float counter;
    
    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealth>();
        bigCharacter = FindObjectOfType<HeroController>();
        heroRb2D = bigCharacter.GetComponent<Rigidbody2D>();
    }
    
       
    
    void Start()
    {
        mouseWorldPos = transform.position;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && isSelected)
        {
            mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            //Debug.Log(mouseWorldPos);
        }

        //Debug.Log(mouseWorldPos);
        //transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speedspeedspeed * Time.deltaTime);
        //AddForce(Player.transform.forward)
        navMeshAgent.SetDestination(mouseWorldPos);
        navMeshAgent.speed = speedspeedspeed;// * Time.deltaTime;

        if (counter > 0)
            counter = Mathf.Clamp(counter - Time.deltaTime, 0f, float.PositiveInfinity);

    }

    

    void OnMouseOver()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (isSelected)
                isSelected = false;
            //Vector3 mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            //if (Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).x) < 5 && Mathf.Abs((mouseWorldPos - GetComponent<EnemyMovement>().position).y) < 5)
            //{
            //    GetComponent<EnemyMovement>().is_Selected = true;

            //}
            else if (!isSelected)
                isSelected = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger && counter == 0)
        {
            OnAttack();
            counter = 2f;
        }
    }

    void OnAttack()
    {
        //adds force to rigidbodies
        Vector3 direction = bigCharacter.transform.position - transform.position;        
        direction.Normalize();
        
        heroRb2D.AddForce(direction * attackForce);

        bigCharacter.OnDamaged(attackDamage);
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
        Debug.Log("Died");
    }

}
