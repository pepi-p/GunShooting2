using UnityEngine;
using UniRx;
using Zenject;

namespace Player
{
    public class GunShot : MonoBehaviour
    {
        [Inject]
        private GunModel _gunModel;
        [SerializeField]
        private Raycast _raycast;

        /// <summary>
        /// 銃のリロードをするクラス
        /// </summary>
        private void Start()
        {
            // クリック
            Observable.EveryUpdate()
                .Where(_ => Input.GetMouseButton(0))
                .Subscribe(_ => Shot())
                .AddTo(gameObject);
        }

        /// <summary>
        /// 射撃
        /// </summary>
        private void Shot()
        {
            if (!_gunModel.ShotEnable) return;
            _gunModel.ReduceAmmo();
            _gunModel.Cooldown();
            
            var hit = _raycast.PointerRay();
            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IDamage>(out var target))
                {
                    target.AddDamage(_gunModel.ShotDamage);
                    // scoreManager.shotHitCount++;
                }
            }
        }
    }
}