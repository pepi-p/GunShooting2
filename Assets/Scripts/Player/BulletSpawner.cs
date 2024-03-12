using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Player
{
    public class BulletSpawner : MonoBehaviour
    {
        [Inject] private Bullet.Factory _bulletFactory;
        [SerializeField] private GameObject playerObj;
        private List<Bullet> bullets = new List<Bullet>();

        /// <summary>
        /// 弾の生成
        /// </summary>
        public void Spawn()
        {
            var bullet = _bulletFactory.Create(playerObj);
            bullets.Add(bullet);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Spawn();
            }
        }
    }
}