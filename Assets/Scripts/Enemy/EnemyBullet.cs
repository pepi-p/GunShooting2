using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start()
    {
        StartCoroutine(DestoryBullet());
    }

    private void Update()
    {
        this.transform.position += speed * Time.deltaTime * this.transform.forward;
    }

    private IEnumerator DestoryBullet()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
