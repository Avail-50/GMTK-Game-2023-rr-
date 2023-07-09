using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class HeroController : MonoBehaviour
{
    [SerializeField] FloatingHealth healthBar;
    public GameObject weaponHitboxPrefab;
    private Collider2D weaponHitbox;

    public float maxCounter;
    private float counter;
    [SerializeField] FloatingStamina staminaBar;

    private GameObject child;

    public int attackDamage;
    public float attackForce;
    public int maxHealth;
    private int health;


    private EnemyController closestEnemy;

    private void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealth>();
        weaponHitbox = Instantiate(weaponHitboxPrefab, transform).GetComponent<Collider2D>();
        counter = maxCounter;
        staminaBar = GetComponentInChildren<FloatingStamina>();
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

        if (counter < maxCounter)
        {
            counter = Mathf.Clamp(counter + Time.deltaTime, 0f, maxCounter);
            staminaBar.UpdateStaminaBar(counter, maxCounter);
        }

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

        staminaBar.UpdateStaminaBar(counter, maxCounter);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger && counter == maxCounter)
        {
            OnAttack();
            counter = 0f;
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
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }

}
