using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform bigCharacter;
    public int force;
    private Rigidbody2D rb2D;
    private NavMeshAgent navMeshAgent;
    [SerializeField] FloatingHealth healthBar;

    [SerializeField] HeroController dealDamage;

    public int attack;

    public int maxHealth;
    public int health;
    //public CameraMove camera;

    public bool is_Selected = false;
    private Vector3 mouseWorldPos;
    //public Vector3 position = transform.position;
    public float speedspeedspeed;
    // Start is called before the first frame update

    public float counter;

    public GameObject prefab;
    
    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealth>();
        dealDamage = bigCharacter.GetComponent<HeroController>();
        rb2D = bigCharacter.GetComponent<Rigidbody2D>();
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
        if (Input.GetButtonDown("Fire1") && is_Selected)
        {
            mouseWorldPos = new(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, transform.position.z);
            //Debug.Log(mouseWorldPos);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            is_Selected = false;
        }

        //Debug.Log(mouseWorldPos);
        //transform.position = Vector3.MoveTowards(transform.position, mouseWorldPos, speedspeedspeed * Time.deltaTime);
        //AddForce(Player.transform.forward)
        navMeshAgent.SetDestination(mouseWorldPos);
        navMeshAgent.speed = speedspeedspeed;// * Time.deltaTime;

        if (counter > 0)
            counter = Mathf.Clamp(counter - Time.deltaTime, 0f, 10f);

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

    void OnTriggerStay2D(Collider2D bigCharacter)
    {

        if (counter == 0)
        {
            OnAttack();
            counter = 2f;
        }



    }

    void OnAttack()
    {
        Debug.Log("Attack");

        //adds force to rigidbodies
        Vector3 direction = bigCharacter.transform.position - transform.position;        
        direction.Normalize();
        
        rb2D.AddForce(direction * force);
        dealDamage.OnDamaged(attack);

    }

    public void OnDamaged(int damageTaken)
    {
        health -= damageTaken;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
            Die();
    }

    public void OnCollect(int buff)
    {
        attack += buff;
    }

    void Die()
    {
        GameObject AttackUp = Instantiate(prefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }

}
