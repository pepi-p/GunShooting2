using UnityEngine;
using Zenject;

namespace Player
{
    public class BulletInstaller : MonoInstaller
    {
        [SerializeField] private GameObject playerObj;
        [SerializeField] private GameObject bulletPrefab;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<GameObject, Bullet, Bullet.Factory>()
                .FromComponentInNewPrefab(bulletPrefab);
        }
    }
}