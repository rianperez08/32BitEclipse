using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Player
{
    [SerializeField] GameObject weapon_collision;

    private void Start()
    {
        weapon_collision.SetActive(false);
    }
    private void Update()
    {
        GetPlayerInput();
    }

    private void GetPlayerInput()
    {
        if(Input.GetMouseButtonDown(0)){
            Debug.Log("Player Attacked!");
            weapon_collision.GetComponent<WeaponHit>().damage = normal_damage;
            UpdatePlayerState(PlayerState.Attack);
        }
    }
    protected override IEnumerator Attack()
    {
        weapon_collision.SetActive(true);
        yield return new WaitForSeconds(attack_duration);
        weapon_collision.SetActive(false);
    }
}
