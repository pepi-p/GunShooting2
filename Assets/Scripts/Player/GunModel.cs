using System;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

namespace Player
{
    /// <summary>
    /// プレイヤーの銃周りのステータス
    /// </summary>
    public class GunModel
    {
        private int ammo = 100;            // 残弾
        private int maxAmmo = 100;         // 最大弾数
        private float shotInterval = 0.1f; // 発射間隔
        private float shotDamage = 1.0f;   // 与ダメージ
        private bool isReload = false;     // リロード中かどうか
        private bool shotEnable = true;    // 撃てるかどうか
        
        public float ShotDamage
        {
            get { return shotDamage; }
        }
        
        public bool ShotEnable
        {
            get { return shotEnable; }
        }

        // 残弾の変更を通知
        private BehaviorSubject<int> ammoChanged;
        public IObservable<int> AmmoChanged => ammoChanged;
        
        // リロードを通知
        private BehaviorSubject<bool> reloaded;
        public IObservable<bool> Reloaded => reloaded;

        public GunModel()
        {
            ammoChanged = new BehaviorSubject<int>(ammo);
            reloaded = new BehaviorSubject<bool>(isReload);
        }
        
        /// <summary>
        /// 弾を1だけ減らす
        /// </summary>
        public void ReduceAmmo()
        {
            if (ammo > 0) ammo--;
            else shotEnable = false;
            ammoChanged.OnNext(ammo);
        }

        /// <summary>
        /// shotIntervalの時間だけ撃てなくする
        /// </summary>
        public async void Cooldown()
        {
            shotEnable = false;
            await UniTask.Delay(TimeSpan.FromSeconds(shotInterval));
            if (!isReload && ammo > 0) shotEnable = true;
        }

        /// <summary>
        /// リロードの開始
        /// </summary>
        public void ReloadStart()
        {
            isReload = true;
            reloaded.OnNext(isReload);
            shotEnable = false;
        }
        
        /// <summary>
        /// リロードの終了
        /// </summary>
        public void ReloadEnd()
        {
            isReload = false;
            reloaded.OnNext(isReload);
            ResetAmmo();
            shotEnable = true;
        }
        
        /// <summary>
        /// 残弾を最大数にリセットする
        /// </summary>
        private void ResetAmmo()
        {
            ammo = maxAmmo;
            ammoChanged.OnNext(ammo);
        }
    }
}