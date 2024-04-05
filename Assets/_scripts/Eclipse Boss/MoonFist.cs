using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public enum MoonFistState
{
    Move,
    Charge,
    Shoot,
    Return,
    Stuck,
    Far
}

public class MoonFist : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    float distancePercentage = 0f;
    float splineLength;

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
    [SerializeField] float max_distance;
    Renderer render;
    Transform player;
    Vector3 initial_pos, target_pos;
    float direction;
    bool isSmash, isStuck, isMoving, isReturning, isShooting,isFallingDown;
    int smash_count,shoot_count;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        isSmash = isStuck = isReturning = isFallingDown = false;
        isMoving = true;
        shoot_count = 0;
        smash_count = 0;
        direction = 1;
        splineLength = spline.CalculateLength();
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
        if (isFallingDown)
            FallDown();
        if (isReturning)
            MoveBack();
    }

    private void UpdateEnemyState(MoonFistState enemyState)
    {
        Debug.Log("MoonFist:" + enemyState);
        switch (enemyState)
        {
            case MoonFistState.Move:
                isReturning = false;
                SetPositions();
                StartCoroutine(MoveState());
                break;
            case MoonFistState.Charge:
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
            case MoonFistState.Return:
                isSmash = false;
                isShooting = false;
                isReturning = true;
                break;
            case MoonFistState.Far:
                StartCoroutine(FarState());
                break;
        }
    }

    #region Movement

    private IEnumerator MoveState()
    {
        isMoving = true;
        yield return new WaitForSeconds(moving_time);
        isMoving = false;
        direction *= -1;
        SetPositions();
        float distance = Vector3.Distance(transform.position, target_pos);
        if (distance >= max_distance)
            UpdateEnemyState(MoonFistState.Far);
        else
            UpdateEnemyState(MoonFistState.Charge);
    }

    private void Move()
    {
        float currentSpeed = Mathf.Clamp(speed * direction, -5, 5);
        distancePercentage += currentSpeed * Time.deltaTime / splineLength;
        distancePercentage = Mathf.Repeat(distancePercentage, 1f);
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
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
        SetPositions();
        targeter.LookAt(target_pos);
        Instantiate(bullet, transform.position, targeter.rotation);
        targeter.LookAt(target_pos + new Vector3(0, 0, 5));
        Instantiate(bullet, transform.position, targeter.rotation);
        targeter.LookAt(target_pos + new Vector3(0, 0, -5));
        Instantiate(bullet, transform.position, targeter.rotation);
    }
    #endregion

    IEnumerator FarState()
    {
        float time = charge_time / 3;
        transform.localScale += Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale -= Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale += Vector3.one;
        yield return new WaitForSeconds(time);
        transform.localScale -= Vector3.one;
        SetPositions();
        transform.position = new Vector3(target_pos.x, transform.position.y, target_pos.z);
        yield return new WaitForSeconds(.1f);
        isFallingDown = true;
        yield return new WaitForSeconds(.5f);
        isFallingDown = false;
        StartCoroutine(GetStuckMelee());
    }

    private void FallDown()
    {
        if (transform.position.y > target_pos.y)
            transform.Translate(0, -10 * Time.deltaTime, 0);
    }

    private void SetPositions()
    {
        target_pos = player.position;
        initial_pos = transform.position;
    }

    private IEnumerator GetStuckMelee()
    {
        isStuck = true;
        render.material = weak;
        yield return new WaitForSeconds(stuck_time);
        isStuck = false;
        render.material = normal;
        UpdateEnemyState(MoonFistState.Return);
    }
    private IEnumerator GetStuck()
    {
        isStuck = true;
        render.material = weak;
        yield return new WaitForSeconds(stuck_time);
        isStuck = false;
        render.material = normal;
        UpdateEnemyState(MoonFistState.Move);
    }
    #region Return
    private void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, initial_pos, speed * Time.deltaTime);
        if (Vector3.Distance(initial_pos, transform.position) <= .001f) UpdateEnemyState(MoonFistState.Move);
    }
    #endregion
}
