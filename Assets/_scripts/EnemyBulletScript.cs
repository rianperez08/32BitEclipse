using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player got hit by projectile!");
        }
        if (!other.CompareTag("Projectile") && !other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
