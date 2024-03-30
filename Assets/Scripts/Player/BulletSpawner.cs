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
        public void Spawn(Vector3 hitPoint)
        {
            // 生成済みで使用していない物があれば再利用
            foreach (var b in bullets)
            {
                if (!b.Enable)
                {
                    b.InitBullet(hitPoint);
                    return;
                }
            }
            
            // なければ新規で生成
            var bullet = _bulletFactory.Create(playerObj, hitPoint);
            bullets.Add(bullet);
        }
    }
}