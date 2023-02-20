using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get; private set; }

    public Stat damage;
    public Stat armor;

    public Stat stamina;
    public Stat strength;
    public Stat intelligence;
    public Stat agility;
    public Stat dexterity;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
    }

    // Percentual armor reduction: damage = damage * 100/(100+armor)
    public void TakeDamage(float damage)
    {
        damage *= (100 / (100 + (float)armor.GetValue()));

        damage = Mathf.Clamp(damage, 0, int.MaxValue);
        damage = Mathf.Floor(damage);

        currentHealth -= (int)damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // TODO: Die in some way

        Debug.Log(transform.name + " died.");
    }
}
