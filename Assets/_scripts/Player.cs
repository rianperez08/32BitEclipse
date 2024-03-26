using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Attack,
    ChargeAttack,
    Parry,
    Invulnerable,
    GetDamaged
}

public abstract class Player : MonoBehaviour
{
    [Header("Player Stats")]
    public int health;
    public int normal_damage;
    public int charged_damage;
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
            case PlayerState.Parry:
                UpdatePlayerState(PlayerState.Invulnerable);
                break;
        }
    }

    protected virtual IEnumerator Invulnerable()
    {
        return null;
    }
    protected virtual IEnumerator Attack()
    {
        return null;
    }
    protected virtual IEnumerator ChargeAttack()
    {
        return null;
    }
    protected virtual IEnumerator Parry()
    {
        return null;
    }
}
