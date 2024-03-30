using UniRx;
using Zenject;

namespace Boss
{
    /// <summary>
    /// フェーズの管理をする
    /// </summary>
    public class BossPhase
    {
        [Inject] private BossStatus _bossStatus;
        public Phase phase { get; private set; } // 現在プレイしているフェーズ

        private void Start()
        {
            // HPが0になったらを呼ぶ
            /*
            _bossStatus
                .PhaseHPChanged
                .Where(hp => hp.GetHPRate() <= 0)
                .Subscribe(_ => );
            */
        }

        /// <summary>
        /// 次のフェーズへ進める
        /// </summary>
        public void NextPhase()
        {
            phase++;
        }
        
        // private void EndPhase()
    }
}