using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// プレイヤーが発射する弾
    /// </summary>
    public class PlayerBullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        private float currentSpeed = 0;

        private void Start()
        {
            StartCoroutine(DestoryBullet());
            currentSpeed = speed * 0.1f;
        }

        private void Update()
        {
            this.transform.position += this.transform.forward * (currentSpeed * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 15);
        }

        private IEnumerator DestoryBullet()
        {
            yield return new WaitForSeconds(1.5f);
            Destroy(this.gameObject);
        }
    }
}