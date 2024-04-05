using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public enum FistState
{
    Move,
    Charge,
    Smash,
    Return,
    Stuck,
    Eclipse
}

public class Eclipse : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    float distancePercentage = 0f;
    float splineLength;

    [Header("SunFist Stats")]
    [SerializeField] float speed;
    [SerializeField] float smash_speed;
    [SerializeField] float moving_time;
    [SerializeField] float smash_time;
    [SerializeField] float stuck_time;
    [SerializeField] float charge_time;
    [SerializeField] float shoot_time;
    [Header("SunFist Mats")]
    [SerializeField] Material normal;
    [SerializeField] Material weak;
    [Header("MoonFist Range")]
    [SerializeField] Transform targeter;
    [SerializeField] GameObject bullet;
    [SerializeField] float max_distance;
    Renderer render;
    Transform player;
    Vector3 initial_pos;
    Vector3 target_pos;
    float direction;
    bool isSmash, isStuck, isMoving, isReturning, isShooting, isFallingDown;
    int smash_count;
    int shoot_count;

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
        SetPositions();
        UpdateEnemyState(FistState.Eclipse);
    }

    private void Update()
    {
        if (isStuck)
            transform.Translate(0, -.01f * Time.deltaTime, 0);
        if (isMoving)
            Move();
        if (isSmash)
            SmashMove();
        if (isReturning)
            MoveBack();
        if (isFallingDown)
            FallDown();
    }

    private void UpdateEnemyState(FistState enemyState)
    {
        Debug.Log("Eclipse:" + enemyState);
        switch (enemyState)
        {
            case FistState.Move:
                isReturning = false;
                StartCoroutine(MoveState());
                break;
            case FistState.Charge:
                StartCoroutine(ChargeState());
                break;
            case FistState.Smash:
                isSmash = true;
                smash_count++;
                SetPositions();
                break;
            case FistState.Return:
                isSmash = false;
                isReturning = true;
                break;
            case FistState.Stuck:
                isSmash = false;
                isReturning = false;
                StartCoroutine(GetStuck());
                break;
            case FistState.Eclipse:
                StartCoroutine(EclipseState());
                break;
        }
    }

    IEnumerator EclipseState()
    {
        direction *= -1;
        shoot_count = smash_count = 0;
        isMoving = true;
        yield return new WaitForSeconds(moving_time);
        isMoving = false;
        Shoot();
        yield return new WaitForSeconds(.5f);
        Shoot();
        yield return new WaitForSeconds(1f);
        isSmash = true;
        yield return new WaitForSeconds(2f);
        isSmash = true;
    }


    #region Movement

    private IEnumerator MoveState()
    {
        isMoving = true;
        yield return new WaitForSeconds(moving_time);
        isMoving = false;
        direction *= -1;
        SetPositions();
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
        UpdateEnemyState(FistState.Smash);
    }
    #endregion

    #region Smash
    private void SmashMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, target_pos, smash_speed * Time.deltaTime);
        if (Vector3.Distance(target_pos, transform.position) <= .001f)
        {
            smash_count++;
            if (smash_count >= 2)
            {
                UpdateEnemyState(FistState.Stuck);
            }
            else
            {
                UpdateEnemyState(FistState.Return);
            }
        }
    }
    #endregion

    #region Return
    private void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, initial_pos, speed * Time.deltaTime);
        if (Vector3.Distance(initial_pos, transform.position) <= .001f)
        {
            if(smash_count >= 2)
            {
                smash_count = 0;
                UpdateEnemyState(FistState.Eclipse);
            }
        }
    }
    #endregion

    #region Shoot

    private void Shoot()
    {
        SetPositions();
        targeter.LookAt(target_pos);
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
        isStuck = true;
        render.material = weak;
        yield return new WaitForSeconds(stuck_time);
        isStuck = false;
        render.material = normal;
        UpdateEnemyState(FistState.Return);
    }

    private void FallDown()
    {
        if (transform.position.y > target_pos.y)
            transform.Translate(0, -10 * Time.deltaTime, 0);
    }
}
