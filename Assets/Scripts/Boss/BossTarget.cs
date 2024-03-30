using UnityEngine;
using Zenject;

namespace Boss
{
    /// <summary>
    /// ボスへダメージを伝えるクラス
    /// NOTE: コライダーがついてるオブジェクトにアタッチして使う
    /// </summary>
    public class BossTarget : MonoBehaviour, IDamage
    {
        [Inject] private BossStatus _bossStatus;
        
        /// <summary>
        /// ボスへダメージを伝える(ray発射側から呼ばれる)
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void AddDamage(float damage)
        {
            _bossStatus.AddDamage(damage);
        }
    }
}