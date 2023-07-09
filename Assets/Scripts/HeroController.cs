using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    [SerializeField] FloatingHealth healthBar;

    public float counter = 4f;
    public float maxCounter = 4f;
    [SerializeField] FloatingStamina staminaBar;

    private GameObject child;


    private void Awake()
    {
        health = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealth>();

        counter = maxCounter;
        staminaBar = GetComponentInChildren<FloatingStamina>();

        child = transform.Find("Aim").gameObject;
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

        //if (counter > 0)
        //   counter = Mathf.Clamp(counter - Time.deltaTime, 0f, 10f);
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.transform.position + new(0, 75f, 0);
        AimAndAttack aimAndAttack = child.GetComponent<AimAndAttack>();

        if (aimAndAttack.onAttackCalled == true)
        {
            counter = 0;
            staminaBar.UpdateStaminaBar(counter, maxCounter);
            aimAndAttack.onAttackCalled = false;
        }            
        else if (counter < maxCounter)
        {
            counter = Mathf.Clamp(counter + Time.deltaTime, 0f, maxCounter);
            staminaBar.UpdateStaminaBar(counter, maxCounter);
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
        //Debug.Log("Hero Died");
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }

}
