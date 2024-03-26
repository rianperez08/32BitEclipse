using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHand : Enemy
{
    //[Header("Hand Stuff")]
    //[SerializeField] Transform target;
    //[SerializeField] float speed;
    //Vector3 initial_position;
    //Vector3 target_position;
    //bool isAttacking;
    //
    //private void Start()
    //{
    //    target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    //    initial_position = transform.position;
    //}
    //
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.T))
    //    {
    //        StartCoroutine(Attack());
    //    }
    //
    //    if (isAttacking) MoveToTarget();
    //    else MoveBackToPos();
    //}
    //
    //private void MoveBackToPos()
    //{
    //    var dir = (initial_position - transform.position).normalized;
    //
    //    transform.position += dir * speed * Time.deltaTime;
    //}
    //
    //private void MoveToTarget()
    //{
    //    var dir = (target_position - transform.position).normalized;
    //
    //    transform.position += dir * speed * Time.deltaTime;
    //}
    //
    //protected override IEnumerator Attack()
    //{
    //    isAttacking = true;
    //    target_position = target.position;
    //    initial_position = transform.position;
    //    yield return new WaitForSeconds(attack_duration);
    //    isAttacking = false;
    //}

}
