using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullethellSpawner : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] GameObject bullet;
    [SerializeField] float rotation_speed;
    [SerializeField] float rate_of_fire;
    float time_before_firing;

    private void Start()
    {
        time_before_firing = rate_of_fire;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Fire()
    {
        FireBullets();
    }

    void AutomaticFire()
    {
        time_before_firing -= Time.deltaTime;

        if (time_before_firing <= 0)
        {
            FireBullets();
            time_before_firing = rate_of_fire;
        }

        RotateSpawner();
    }

    private void RotateSpawner()
    {
        transform.Rotate(0, 0, rotation_speed * Time.deltaTime);
    }

    private void FireBullets()
    {
        Instantiate(bullet, transform.position, transform.rotation);
        Quaternion secondRotation = transform.rotation * Quaternion.Euler(0, 0, 90);
        Instantiate(bullet, transform.position, secondRotation);
    }
}
