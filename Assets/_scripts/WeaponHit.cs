using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider weapon)
    {
        if (weapon.CompareTag("Enemy"))
        {
            Debug.Log("Enemy took "+ damage + " damage!");
        }
    }
}
