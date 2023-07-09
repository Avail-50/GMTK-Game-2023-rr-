using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AimAndAttack : MonoBehaviour
{
    private BoxCollider2D aim;
    private Transform enemy;
    
    public float force;
    private Rigidbody2D rb2D;
    private EnemyMovement dealDamage;
    //private bool targetLocked = false;
    public int attack;
    public bool onAttackCalled = false;
    public GameObject parent;
    private HeroController heroController;

    //[SerializeField] FloatingStamina staminaBar;

    // Start is called before the first frame update
    void Awake()
    {

        parent = transform.parent.gameObject;
        heroController = parent.GetComponent<HeroController>();
        //counter = maxCounter;
        //staminaBar = GetComponentInChildren<FloatingStamina>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (counter < maxCounter)
        //{
        //    counter = Mathf.Clamp(counter + Time.deltaTime, 0f, maxCounter);
        //    staminaBar.UpdateStaminaBar(counter, maxCounter);
        //
        //}

        //if (enemy == null)
        //{
        DetectNearestEnemy();

    }

    void OnAttack()
    {
        Debug.Log("Big Attack");

        //adds force to rigidbodies
        Vector3 direction = enemy.transform.position - transform.position;
        direction.Normalize();

        rb2D.AddForce(direction * force);

        dealDamage.OnDamaged(attack);


    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (heroController.counter == 4)
        {
            OnAttack();
            onAttackCalled = true;
            //counter = 0f;
            //staminaBar.UpdateStaminaBar(counter, maxCounter);
        }

    }

    void DetectNearestEnemy()
    {
        var foundEnemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);
        EnemyMovement nearestEnemy = foundEnemies.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).First();
        //Collider[] nearbyRigidbodies = Physics.OverlapSphere(transform.position, radius);
        //if (nearbyRigidbodies != null)
        //targetLocked = true;
        enemy = nearestEnemy.transform;

        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, enemy.position - transform.position);
        transform.rotation = rotation;
        rb2D = enemy.GetComponent<Rigidbody2D>();
        dealDamage = enemy.GetComponent<EnemyMovement>();
    }
}
