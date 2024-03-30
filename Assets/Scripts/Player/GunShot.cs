using UnityEngine;
using Zenject;
using UniRx;
using UniRx.Triggers;

namespace Player
{
    /// <summary>
    /// 銃を撃つクラス
    /// </summary>
    public class GunShot : MonoBehaviour
    {
        [Inject] private IInputProvider _inputProvider;
        [Inject] private GunModel _gunModel;
        [SerializeField] private BulletSpawner _bulletSpawner;

        private Camera cam; // メインカメラ

        private void Start()
        {
            // トリガー入力の監視
            this.UpdateAsObservable()
                .Where(_ => _inputProvider.GetTrigger())
                .Subscribe(_ => Shot())
                .AddTo(this);

            cam = Camera.main;
        }

        /// <summary>
        /// 射撃
        /// </summary>
        private void Shot()
        {
            if (!_gunModel.ShotEnable) return;
            _gunModel.ReduceAmmo();
            _gunModel.Cooldown();

            var isHit = Physics.Raycast(cam.ScreenPointToRay(_inputProvider.GetPointerPos()), out var hit, 50f, 1);
            if (!isHit) return;

            // 弾のモデルを生成
            _bulletSpawner.Spawn(hit.point);

            // ダメージを与える
            if (hit.collider.TryGetComponent<IDamage>(out var target))
            {
                target.AddDamage(_gunModel.ShotDamage);
                // scoreManager.shotHitCount++;
            }
        }
    }
}