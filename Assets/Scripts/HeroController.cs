using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HeroController : MonoBehaviour
{
    [SerializeField] FloatingHealth healthBar;
    public GameObject weaponHitboxPrefab;
    private Collider2D weaponHitbox;

    public int attackDamage;
    public float attackForce;
    public int maxHealth;

    [SerializeField] int health;

    private EnemyController closestEnemy;
    private float counter = 0;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealth>();
        weaponHitbox = Instantiate(weaponHitboxPrefab, transform).GetComponent<Collider2D>();
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

        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        closestEnemy = enemies.OrderBy(x => (x.transform.position - transform.position).sqrMagnitude).First();
        if (closestEnemy == null) return;
        weaponHitbox.transform.rotation = Quaternion.LookRotation(Vector3.forward, closestEnemy.transform.position - transform.position);
    }

    void OnAttack()
    {
        //adds force to rigidbodies
        Vector3 direction = closestEnemy.transform.position - transform.position;
        direction.Normalize();

        closestEnemy.GetComponent<Rigidbody2D>().AddForce(direction * attackForce);
        closestEnemy.OnDamaged(attackDamage);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger && counter == 0)
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
