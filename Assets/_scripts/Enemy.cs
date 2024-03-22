using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Charging,
    Attack
}
public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health;
    public int normal_damage;
    public int collision_damage;
    [Header("Enemy Combat")]
    public float attack_cooldown;
    public float attack_duration;
    public float charge_duration;

    public void UpdateEnemyState(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Attack:
                StartCoroutine(Attack());
                break;
            case EnemyState.Charging:
                StartCoroutine(Charging());
                break;
        }
    }
    protected virtual IEnumerator Charging()
    {
        yield return new WaitForSeconds(charge_duration);
    }
    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(attack_duration);
    }
}
