using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Zenject;

namespace Boss
{
    /// <summary>
    /// ボスのUIを表示するクラス
    /// </summary>
    public class BossUI : MonoBehaviour
    {
        [Inject] private BossStatus _bossStatus;
        [SerializeField] private Image hpBar;
        
        private void Start()
        {
            // 左ガトリングのHPを購読
            _bossStatus
                .PhaseHPChanged
                .Subscribe(hp => DisplayHP(hp.GetHPRate()));
        }

        /// <summary>
        /// HPバーを更新
        /// </summary>
        /// <param name="rate">HP割合</param>
        private void DisplayHP(float rate)
        {
            hpBar.fillAmount = rate;
        }
    }
}