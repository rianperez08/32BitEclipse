using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFist : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float stuck_time;
    Transform player;
    Vector3 initial_pos;
    Vector3 target_pos;
    bool isSmash;
    int smash_count;

    private void Awake()
    {

    }

    private void Update()
    {
        FirstPhase();
    }

    private void FirstPhase()
    {
        StartCoroutine(Smash());
        if (smash_count >= 2)
        {
            StartCoroutine(GetStuck());
        }
    }
    private IEnumerator Smash()
    {
        yield return new WaitForSeconds(stuck_time);
    }

    private IEnumerator GetStuck()
    {
        yield return new WaitForSeconds(stuck_time);
    }
}
