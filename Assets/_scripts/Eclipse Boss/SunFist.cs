using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public enum SunFistState
{
    Move,
    Charge,
    Smash,
    Return,
    Stuck,
}

public class SunFist : MonoBehaviour
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
    [Header("SunFist Mats")]
    [SerializeField] Material normal;
    [SerializeField] Material weak;
    [Header("MoonFist Range")]
    [SerializeField] Transform targeter;
    [SerializeField] GameObject bullet;
    Renderer render;
    Transform player;
    Vector3 initial_pos;
    Vector3 target_pos;
    Vector3 move_direction = new(1,0,0);
    bool isSmash, isStuck, isMoving , isReturning;
    int smash_count;

    private void Awake()
    {
        isSmash = isStuck = isReturning = false;
        isMoving = true;
        smash_count = 0;
        player = GameObject.Find("Player").GetComponent<Transform>();
        render = GetComponent<Renderer>();
    }

    private void Start()
    {
        splineLength = spline.CalculateLength();
        UpdateEnemyState(SunFistState.Move);
        SetPositions();
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
    }

    private void UpdateEnemyState(SunFistState enemyState)
    {
        switch (enemyState)
        {
            case SunFistState.Move:
                isReturning = false;
                Debug.Log("Sunfist: Started Moving");
                SetPositions();
                StartCoroutine(MoveState());
                break;
            case SunFistState.Charge:
                StartCoroutine(ChargeState());
                break;
            case SunFistState.Smash:
                isSmash = true;
                smash_count++;
                SetPositions();
                Debug.Log("Sunfist: Started Smash "+smash_count);
                //StartCoroutine(SmashState());
                break;
            case SunFistState.Return:
                isSmash = false;
                isReturning = true;
                Debug.Log("Sunfist: Started Return");
                //StartCoroutine(ReturnState());
                break;
            case SunFistState.Stuck:
                isSmash = false;
                smash_count = 0;
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
        UpdateEnemyState(SunFistState.Charge);
    }

    private void Move()
    {
        if(transform.position.x <= target_pos.x - 5)
            move_direction.x = 1;
        if (transform.position.x >= target_pos.x + 5)
            move_direction.x = -1;

        float currentSpeed = Mathf.Clamp(speed * move_direction.x, -speed, speed);
        //transform.position += speed * Time.deltaTime * move_direction; 
        distancePercentage += speed* Time.deltaTime / splineLength;
        distancePercentage += Mathf.Repeat(distancePercentage, 1f);

        Vector3 currentPosition = spline.EvaluatePosition(speed * Time.deltaTime);
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
        UpdateEnemyState(SunFistState.Smash);
    }
    #endregion

    #region Smash
    private void SmashMove()
    {
        transform.position = Vector3.MoveTowards(transform.position, target_pos, smash_speed * Time.deltaTime);
        if (Vector3.Distance(target_pos, transform.position) <= .001f)
        {
            if(smash_count >= 2) UpdateEnemyState(SunFistState.Stuck);
            else UpdateEnemyState(SunFistState.Return);
        }
    }
        #endregion

        #region Return
        private void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, initial_pos, speed * Time.deltaTime);
        if (Vector3.Distance(initial_pos, transform.position) <= .001f) UpdateEnemyState(SunFistState.Move);
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
        UpdateEnemyState(SunFistState.Return);
    }

    #region Archive
    //private IEnumerator SmashState()
    //{
    //    Debug.Log("Sunfist: Smash");
    //    SetPositions();
    //    isSmash = true;
    //    yield return new WaitForSeconds(smash_time);
    //    isSmash = false;
    //    UpdateEnemyState(SunFistState.Return);
    //}
    //private IEnumerator ReturnState()
    //{
    //    isReturning = true;
    //    yield return new WaitForSeconds(smash_time);
    //    isReturning = false;
    //    UpdateEnemyState(SunFistState.Move);
    //}
    #endregion
}
