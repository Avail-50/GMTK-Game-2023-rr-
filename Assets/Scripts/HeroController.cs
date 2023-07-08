using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public int maxHealth;
    public int health;
    [SerializeField] FloatingHealth healthBar;
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
        //transform.rotation = camera.transform.rotation;
        //transform.position = target.transform.position + new(0, 75f, 0);
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
