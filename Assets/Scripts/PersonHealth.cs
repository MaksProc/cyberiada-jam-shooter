using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonHealth : MonoBehaviour
{
    [SerializeField] GameObject person;
    [SerializeField] float health;

    public void GetDamage(float damdage) 
    {
        health -= damdage;

        if (health <= 0) Die();
    }

    public void Die() 
    {
        Destroy(person);
        Debug.Log("Person is dead...");
    }
}
