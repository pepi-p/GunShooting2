using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    /*
    [SerializeField] private Player player;
    [SerializeField] private EnemyBullet bullet;
    [SerializeField] private Transform muzzle;
    public float randomRange;
    public float shotDamage;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    public void ShotBullet()
    {
        var randomOffset = Random.Range(-randomRange, randomRange);
        muzzle.rotation = Quaternion.LookRotation((player.transform.position + Vector3.up * 1.2f + cam.transform.right * randomOffset) - muzzle.transform.position);
        Instantiate(bullet, muzzle.transform.position, Quaternion.LookRotation(muzzle.transform.forward));
        if (Mathf.Abs(randomOffset) < 0.05f) player.Hit(shotDamage);
    }
    */
}
