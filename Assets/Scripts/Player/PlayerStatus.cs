using System;
using UniRx;

namespace Player
{
    /// <summary>
    /// プレイヤーのステータス
    /// </summary>
    public class PlayerStatus
    {
        private float hp;              // hp
        private float maxHP;           // 最大HP
        
        // 射撃の許可
        public bool AllowShot { get; set; } = true; 
        
        // 被弾の許可
        public bool AllowDamage { get; set; }
        
        // HPの変更を通知
        private Subject<float> hpChanged;
        public IObservable<float> HPChanged => hpChanged;

        public PlayerStatus()
        {
            hpChanged = new Subject<float>();
        }

        /// <summary>
        /// HPの残量の割合を取得
        /// </summary>
        /// <returns>HP残量割合</returns>
        public float GetHPRate()
        {
            return hp / maxHP;
        }
        
        /// <summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">ダメージ</param>
        public void Damage(float damage)
        {
            if (!AllowDamage) return;
            hp -= damage;
            hpChanged.OnNext(hp);
            // if (hp < 0) ;
        }
    }
}