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
            /*
            // 左ガトリングのHPを購読
            _bossStatus
                .GatlingLHPChanged
                .Subscribe(hp => );
            
            // 右ガトリングのHPを購読
            _bossStatus
                .GatlingRHPChanged
                .Subscribe(hp => );
            
            // 本体のHPを購読
            _bossStatus
                .BodyHPChanged
                .Subscribe(hp => );
                */
        }

        private void DisplayHP()
        {
            
        }
    }
}