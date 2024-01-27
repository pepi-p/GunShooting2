using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Missile : MonoBehaviour
{
    public Player player;
    public GameObject targetUI;
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject explosion;

    private bool isDamage = true;

    private void Update()
    {
        this.transform.position += this.transform.forward * (speed * Time.deltaTime);
        this.transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(this.transform.forward), Quaternion.LookRotation(player.transform.position - this.transform.position), Time.deltaTime * rotateSpeed);
        if (isDamage && Vector3.SqrMagnitude(player.transform.position - this.transform.position) < 4)
        {
            player.Hit(10);
            Instantiate(explosionPrefab, null);
            explosion.SetActive(true);
            explosion.transform.parent = null;
            Destroy(targetUI.gameObject);
            Destroy(this.gameObject);
        }
    }

    public IEnumerator Hit()
    {
        isDamage = false;
        yield return new WaitForSecondsRealtime(0.5f);
        Instantiate(explosionPrefab, null);
        explosion.SetActive(true);
        explosion.transform.parent = null;
        yield return new WaitForSecondsRealtime(1.0f);
        Destroy(targetUI.gameObject);
        Destroy(this.gameObject);
    }
}
