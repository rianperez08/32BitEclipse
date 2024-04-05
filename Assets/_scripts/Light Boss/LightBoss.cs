using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightBossState
{

}
public class LightBoss : MonoBehaviour
{
    GameObject[] torches;
    bool isActive = true;
    bool isShooting = true;

    GameObject player;

    [Header("Cat Properties")]
    public Transform cat_eye;
    [SerializeField] float cat_eye_cooldown;
    [SerializeField] GameObject cat_bullet;

    [Header("Goat Properties")]
    public Transform goat_eye;
    [SerializeField] float goat_eye_cooldown;
    [SerializeField] GameObject goat_bullet;
    [SerializeField] float goat_spread;

    [Header("Frog Properties")]
    public Transform frog_eye;
    [SerializeField] float frog_eye_cooldown;
    [SerializeField] GameObject frog_bullet;
    [SerializeField] float frog_spread;

    [Header("Main Body Properties")]
    [SerializeField] GameObject main_body;

    private void Start()
    {
        isShooting = false;
        torches = GameObject.FindGameObjectsWithTag("Torch");
        player = GameObject.Find("Player");
        CheckTorches();
        StartAllShooting();
    }

    private void Update()
    {
        CheckTorches();
        if (isActive) main_body.SetActive(true);
        else main_body.SetActive(false);
        goat_eye.LookAt(player.transform.position);
        frog_eye.LookAt(player.transform.position);

    }

    void StartAllShooting()
    {
        StartCoroutine(CatShoot());
        StartCoroutine(GoatShoot());
        StartCoroutine(FrogShoot());
    }

    IEnumerator CatShoot()
    {
        GameObject target = FindClosestTorch();
        if (target == null) target = player;
        cat_eye.LookAt(target.transform.position);
        Instantiate(cat_bullet, cat_eye.position, cat_eye.rotation);
        yield return new WaitForSeconds(cat_eye_cooldown);
        StartCoroutine(CatShoot());
    }

    IEnumerator GoatShoot()
    {
        Instantiate(goat_bullet,goat_eye.position,goat_eye.rotation * Quaternion.Euler(0,-goat_spread, 0));
        Instantiate(goat_bullet,goat_eye.position,goat_eye.rotation * Quaternion.Euler(0, goat_spread, 0));
        Instantiate(goat_bullet,goat_eye.position,goat_eye.rotation);
        yield return new WaitForSeconds(goat_eye_cooldown);
        StartCoroutine(GoatShoot());
    }

    IEnumerator FrogShoot()
    {
        if(Random.Range(1, 100) % 2 == 0)
            Instantiate(frog_bullet, frog_eye.position, frog_eye.rotation * Quaternion.Euler(0, frog_spread, 0));
        else
            Instantiate(frog_bullet, frog_eye.position, frog_eye.rotation * Quaternion.Euler(0, -frog_spread, 0));
        yield return new WaitForSeconds(frog_eye_cooldown);
        StartCoroutine(FrogShoot());

    }

    GameObject FindClosestTorch()
    {
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject torch in torches)
        {
            if (!torch.GetComponent<TorchScript>().isOn)
                continue;
            float distance = Vector3.Distance(torch.transform.position, transform.position);

            if (distance < minDistance)
            {
                closest = torch;
                minDistance = distance;
            }
        }
        if (closest != null)
        {
            Debug.Log("Closest object: " + closest.name);
            return closest;
        }

        return null;
    }

    public void CheckTorches()
    {
        foreach (GameObject torch in torches)
        {
            if (!torch.GetComponent<TorchScript>().isOn)
            {
                isActive = false;
                break;
            }
            isActive = true;
        }
    }
}
