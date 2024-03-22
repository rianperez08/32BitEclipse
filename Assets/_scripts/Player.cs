using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Attack,
    ChargeAttack,
    Invulnerable
}

public abstract class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int health;
    public int normal_damage;
    public int charged_damage;

    [Header("Player Combaat")]
    public float attack_cooldown;
    public float attack_duration;
    public float charge_duration;
    public float invul_duration;


    public void UpdatePlayerState(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Attack:
                StartCoroutine(Attack());
                break;
            case PlayerState.ChargeAttack:
                StartCoroutine(ChargeAttack());
                break;
        }
    }

    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(attack_duration);
    }
    protected virtual IEnumerator ChargeAttack()
    {
        yield return new WaitForSeconds(charge_duration);
    }
    protected virtual void Attack(int additional_damage)
    {

    }
}
