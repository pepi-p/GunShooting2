using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 100;
        private float currentSpeed = 0;
        private GameObject _playerObj;

        [Inject]
        public void Construct(GameObject playerObj)
        {
            _playerObj = playerObj;
        }

        public class Factory : PlaceholderFactory<GameObject, Bullet>
        {
            
        }
        
        private void Start()
        {
            currentSpeed = speed * 0.1f;
            this.transform.position = _playerObj.transform.position + Vector3.up * 0.9f;
        }

        private void Update()
        {
            this.transform.position += this.transform.forward * (currentSpeed * Time.deltaTime);
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 15);
        }
    }
}