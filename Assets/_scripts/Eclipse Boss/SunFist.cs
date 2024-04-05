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
    Far,
}

public class SunFist : MonoBehaviour
{
    [Header("Fist Stat")]
    [SerializeField] float speed;
    [SerializeField] float direction;
    
    Vector3 initial_pos;
    Vector3 target_pos;

    float distancePercentage = 0f;
    float splineLength;

    [SerializeField] SplineContainer spline;

    GameObject player;
    bool isMoving;
    private void Awake()
    {
        player = GameObject.Find("Player");
        direction = 1;
        isMoving = true;
    }
    private void Start()
    {
        splineLength = spline.CalculateLength();
        StartCoroutine(ChangeDirection(Random.Range(1, 5)));
    }

    private void Update()
    {
        if (isMoving) MoveFist();
    }

    IEnumerator ChangeDirection(float time)
    {
        yield return new WaitForSeconds(time);
        direction *= -1; 
        StartCoroutine(ChangeDirection(Random.Range(1, 5)));
    }

    private void MoveFist()
    {
        float currentSpeed = Mathf.Clamp(speed * direction, -5, 5);
        distancePercentage += currentSpeed * Time.deltaTime / splineLength;
        distancePercentage = Mathf.Repeat(distancePercentage, 1f);
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;
    }

    private void SetPositions()
    {
        target_pos = player.transform.position;
        initial_pos = transform.position;
    }

    //[SerializeField] SplineContainer spline;
    //float distancePercentage = 0f;
    //float splineLength;

    //[Header("SunFist Stats")]
    //[SerializeField] float speed;
    //[SerializeField] float smash_speed;
    //[SerializeField] float moving_time;
    //[SerializeField] float smash_time;
    //[SerializeField] float stuck_time;
    //[SerializeField] float charge_time;
    //[SerializeField] float shoot_time;
    //[Header("SunFist Mats")]
    //[SerializeField] Material normal;
    //[SerializeField] Material weak;
    //[Header("MoonFist Range")]
    //[SerializeField] Transform targeter;
    //[SerializeField] GameObject bullet;
    //[SerializeField] float max_distance;
    //Renderer render;
    //Transform player;
    //Vector3 initial_pos;
    //Vector3 target_pos;
    //float direction;
    //bool isSmash, isStuck, isMoving , isReturning, isShooting,isFallingDown;
    //int smash_count;
    //int shoot_count;

    //private void Awake()
    //{
    //    player = GameObject.Find("Player").GetComponent<Transform>();
    //    render = GetComponent<Renderer>();
    //}

    //private void Start()
    //{
    //    isSmash = isStuck = isReturning = isFallingDown = false;
    //    isMoving = true;
    //    shoot_count = 0;
    //    smash_count = 0;
    //    direction = 1;
    //    splineLength = spline.CalculateLength();
    //    UpdateEnemyState(SunFistState.Move);
    //    SetPositions();
    //}

    //private void Update()
    //{
    //    if (isStuck)
    //        transform.Translate(0, -.01f * Time.deltaTime, 0);
    //    if (isMoving)
    //        Move();
    //    if (isSmash)
    //        SmashMove();
    //    if (isReturning)
    //        MoveBack();
    //    if (isFallingDown)
    //        FallDown();
    //}

    //private void UpdateEnemyState(SunFistState enemyState)
    //{
    //    //Debug.Log("SunFist:" + enemyState);
    //    switch (enemyState)
    //    {
    //        case SunFistState.Move:
    //            isReturning = false;
    //            StartCoroutine(MoveState());
    //            break;
    //        case SunFistState.Charge:
    //            StartCoroutine(ChargeState());
    //            break;
    //        case SunFistState.Smash:
    //            isSmash = true;
    //            smash_count++;
    //            SetPositions();
    //            //StartCoroutine(SmashState());
    //            break;
    //        case SunFistState.Return:
    //            isSmash = false;
    //            isReturning = true;
    //            //StartCoroutine(ReturnState());
    //            break;
    //        case SunFistState.Stuck:
    //            isSmash = false;
    //            smash_count = 0;
    //            StartCoroutine(GetStuck());
    //            break;
    //        case SunFistState.Far:
    //            StartCoroutine(FarState());
    //            break;
    //    }
    //}


    //#region Movement

    //private IEnumerator MoveState()
    //{
    //    isMoving = true;
    //    yield return new WaitForSeconds(moving_time);
    //    isMoving = false;
    //    direction *= -1;
    //    SetPositions();
    //    float distance = Vector3.Distance(transform.position, target_pos);
    //    if (distance >= max_distance)
    //        UpdateEnemyState(SunFistState.Far);
    //    else
    //        UpdateEnemyState(SunFistState.Charge);
    //}

    //private void Move()
    //{
    //    float currentSpeed = Mathf.Clamp(speed * direction, -5, 5);
    //    distancePercentage += currentSpeed * Time.deltaTime / splineLength;
    //    distancePercentage = Mathf.Repeat(distancePercentage, 1f);
    //    Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
    //    transform.position = currentPosition;
    //}

    //#endregion

    //#region Charge
    //private IEnumerator ChargeState()
    //{
    //    float time = charge_time / 3;
    //    transform.localScale += Vector3.one;
    //    yield return new WaitForSeconds(time);
    //    transform.localScale -= Vector3.one;
    //    yield return new WaitForSeconds(time);
    //    transform.localScale += Vector3.one;
    //    yield return new WaitForSeconds(time);
    //    transform.localScale -= Vector3.one;
    //    UpdateEnemyState(SunFistState.Smash);
    //}
    //#endregion

    //#region Smash
    //private void SmashMove()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, target_pos, smash_speed * Time.deltaTime);
    //    if (Vector3.Distance(target_pos, transform.position) <= .001f)
    //    {
    //        if(smash_count >= 2) UpdateEnemyState(SunFistState.Stuck);
    //        else UpdateEnemyState(SunFistState.Return);
    //    }
    //}
    //    #endregion

    //#region Return
    //private void MoveBack()
    //{
    //    transform.position = Vector3.MoveTowards(transform.position, initial_pos, speed * Time.deltaTime);
    //    if (Vector3.Distance(initial_pos, transform.position) <= .001f) UpdateEnemyState(SunFistState.Move);
    //}
    //#endregion

    //#region Shoot
    //private IEnumerator ShootState()
    //{
    //    isShooting = true;
    //    yield return new WaitForSeconds(shoot_time);
    //    isShooting = false;
    //    if (shoot_count >= 2) UpdateEnemyState(SunFistState.Stuck);
    //    else UpdateEnemyState(SunFistState.Move);
    //}

    //private void Shoot()
    //{
    //    targeter.LookAt(target_pos);
    //    Instantiate(bullet, transform.position, targeter.rotation);
    //}
    //#endregion

    //private void SetPositions()
    //{
    //    target_pos = player.position;
    //    initial_pos = transform.position;
    //}

    //private IEnumerator GetStuck()
    //{
    //    isStuck = true;
    //    render.material = weak;
    //    yield return new WaitForSeconds(stuck_time);
    //    isStuck = false;
    //    render.material = normal;
    //    UpdateEnemyState(SunFistState.Return);
    //}

    //private IEnumerator FarState()
    //{
    //    isMoving = false;
    //    Shoot();
    //    yield return new WaitForSeconds(.5f);
    //    Shoot();
    //    yield return new WaitForSeconds(.5f);
    //    SetPositions();
    //    isFallingDown = true;
    //    yield return new WaitForSeconds(.5f);
    //    isFallingDown = false;
    //    StartCoroutine(GetStuck());
    //}

    //private void FallDown()
    //{
    //    if(transform.position.y > target_pos.y)
    //        transform.Translate(0, -10 * Time.deltaTime, 0);
    //}

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
