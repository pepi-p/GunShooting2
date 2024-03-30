using UnityEngine;
using Player;
using Zenject;

namespace Boss
{
    /// <summary>
    /// タイムラインから他クラスのメソッドを呼ぶためのクラス
    /// </summary>
    public class BossAnimation : MonoBehaviour
    {
        [Inject] private PlayerStatus _playerStatus;
        [SerializeField] private GatlingAttack _gatlingAttack;

        /// <summary>
        /// プレイヤーにダメージを与える
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void AddPlayerDamage(float damage)
        {
            _playerStatus.Damage(damage);
        }

        /// <summary>
        /// ガトリングで攻撃
        /// </summary>
        /// <param name="length">攻撃の長さ</param>
        public void GatlingAttack(float length)
        {
            _gatlingAttack.Attack(length);
        }
    }
}