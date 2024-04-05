using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
    [SerializeField] Light light_source;
    public bool isOn = true;
    private void Start()
    {
        light_source.enabled = true;
    }

    private void Update()
    {
        light_source.enabled = isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Projectile"))
        {
            isOn = false;
            Destroy(other.gameObject);
        }
    }

}
