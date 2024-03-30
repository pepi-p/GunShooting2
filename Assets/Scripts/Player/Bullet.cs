using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 100;
        [SerializeField] private MeshRenderer meshRenderer;
        private float currentSpeed = 0;
        private GameObject _playerObj;
        private Vector3 _destPos;
        public bool Enable { get; private set; }

        [Inject]
        public void Construct(GameObject playerObj, Vector3 destPos)
        {
            _playerObj = playerObj;
            _destPos = destPos;
        }

        public class Factory : PlaceholderFactory<GameObject, Vector3, Bullet>
        {
            
        }
        
        private void Start()
        {
            InitBullet(_destPos);
        }

        private void Update()
        {
            if (!Enable) return;
            
            // 目標を通り過ぎたかどうか（通り過ぎた = 前方向と目標へのベクトルの内積がマイナス）
            var dot = Vector3.Dot(this.transform.forward, _destPos - this.transform.position);

            if (dot < 0)
            {
                Enable = false;
                meshRenderer.enabled = false;
            }
            else
            {
                // NOTE: 徐々に加速しながら前方に進む。速度は一定に収束する
                this.transform.position += this.transform.forward * (currentSpeed * Time.deltaTime);
                currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 15);
            }
        }

        /// <summary>
        /// 弾の初期化
        /// </summary>
        public void InitBullet(Vector3 destPos)
        {
            // 目標を更新
            _destPos = destPos;
            
            // 速度初期化
            currentSpeed = speed * 0.1f;
            
            // 位置初期化
            this.transform.position = _playerObj.transform.position + Vector3.up * 0.9f;
            this.transform.rotation = Quaternion.LookRotation(_destPos - this.transform.position);
            
            meshRenderer.enabled = true;
            Enable = true;
        }
    }
}