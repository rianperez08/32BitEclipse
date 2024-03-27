using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoonFistState
{
    Move,
    Charge,
    Shoot,
    Stuck
}

public class MoonFist : MonoBehaviour
{
    [Header("MoonFist Stats")]
    [SerializeField] float speed;
    [SerializeField] float smash_speed;
    [SerializeField] float moving_time;
    [SerializeField] float smash_time;
    [SerializeField] float stuck_time;
    [SerializeField] float charge_time;
    [SerializeField] float shoot_time;
    [Header("MoonFist Mats")]
    [SerializeField] Material normal;
    [SerializeField] Material weak;
    [Header("MoonFist Range")]
    [SerializeField] Transform targeter;
    [SerializeField] GameObject bullet;
    Renderer render;
    Transform player;
    Vector3 initial_pos;
    Vector3 target_pos;
    Vector3 move_direction = new(1, 0, 0);
    bool isSmash, isStuck, isMoving, isReturning, isShooting;
    int shoot_count;

    private void Awake()
    {
        isSmash = isStuck = isReturning = false;
        isMoving = true;
        shoot_count = 0;
        player = GameObject.Find("Player").GetComponent<Transform>();
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateEnemyState(MoonFistState.Move);
        SetPositions();
    }

    private void Update()
    {
        if (isStuck)
            transform.Translate(0, -.01f * Time.deltaTime, 0);
        if (isMoving)
            Move();
        if (isShooting)
            Shoot();
    }

    private void UpdateEnemyState(MoonFistState enemyState)
    {
        switch (enemyState)
        {
            case MoonFistState.Move:
                Debug.Log("Moonfist: Started Moving");
                SetPositions();
                StartCoroutine(MoveState());
                break;
            case MoonFistState.Charge:
                Debug.Log("Moonfist: Started Moving");
                StartCoroutine(ChargeState());
                break;
            case MoonFistState.Shoot:
                shoot_count++;
                SetPositions();
                StartCoroutine(ShootState());
                break;
            case MoonFistState.Stuck:
                shoot_count = 0;
                StartCoroutine(GetStuck());
                break;
        }
    }

    #region Movement

    private IEnumerator MoveState()
    {
        isMoving = true;
        yield return new WaitForSeconds(moving_time);
        isMoving = false;
        UpdateEnemyState(MoonFistState.Charge);
    }

    private void Move()
    {
        if (transform.position.x <= target_pos.x - 5)
            move_direction.x = 1;
        if (transform.position.x >= target_pos.x + 5)
            move_direction.x = -1;
        transform.position += speed * Time.deltaTime * move_direction;
    }

    #endregion

    #region Charge
    private IEnumerator ChargeState()
    {
        float time = charge_time / 3;
        transform.localScale += Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale -= Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale += Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale -= Vector3.one;
        UpdateEnemyState(MoonFistState.Shoot);
    }
    #endregion

    #region Shoot
    private IEnumerator ShootState()
    {
        targeter.LookAt(target_pos);
        isShooting = true;
        yield return new WaitForSeconds(shoot_time);
        isShooting = false;
        if (shoot_count >= 2) UpdateEnemyState(MoonFistState.Stuck);
        else UpdateEnemyState(MoonFistState.Move);
    }

    private void Shoot()
    {
        Instantiate(bullet, transform.position, targeter.rotation);
    }
    #endregion

    private void SetPositions()
    {
        target_pos = player.position;
        initial_pos = transform.position;
    }

    private IEnumerator GetStuck()
    {
        Debug.Log("Sunfist: Stuck");
        isStuck = true;
        render.material = weak;
        yield return new WaitForSeconds(stuck_time);
        isStuck = false;
        render.material = normal;
        UpdateEnemyState(MoonFistState.Move);
    }
}
