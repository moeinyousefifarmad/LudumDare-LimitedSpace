using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    [SerializeField] private float FireRate;
    [SerializeField] private GameObject bulletPrefab;
    private Transform parentTransform;
    private float timer;
    private void Awake()
    {
        parentTransform = GetComponentInParent<Transform>();
    }

    public void ShootingManual()
    {
        Instantiate(bulletPrefab , transform.position , parentTransform.rotation);
    }

    public void AutomaticShooting()
    {
        timer += Time.deltaTime;
        if(timer >= FireRate)
        {
            Instantiate(bulletPrefab , transform.position , parentTransform.rotation);
            timer = 0;
        }
    }
}
