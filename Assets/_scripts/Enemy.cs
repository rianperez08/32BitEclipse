using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Invulnerable,
    Attack
}
public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health;
    public int normal_damage;
    public int collision_damage;

    [Header("Enemy Stuff")]
    public float invulnerable_time;
    public void UpdateEnemyState(EnemyState enemyState)
    {
        switch (enemyState)
        {
            case EnemyState.Invulnerable:
                StartCoroutine(Invulnerable());
                break;
        }
    }
    protected virtual IEnumerator Invulnerable()
    {
        yield return new WaitForSeconds(invulnerable_time);
    }
}
