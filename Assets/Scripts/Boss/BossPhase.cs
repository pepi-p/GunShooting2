using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace Boss
{
    /// <summary>
    /// フェーズの管理をする
    /// </summary>
    public class BossPhase
    {
        [Inject] private BossStatus _bossStatus;
        [SerializeField] private BossTimelineController _bossTimeline;
        public Phase phase { get; private set; } // 現在プレイしているフェーズ

        private void Start()
        {
            // HPが0になったら次のフェーズへ進める
            _bossStatus
                .PhaseHPChanged
                .Where(hp => hp.GetHPRate() <= 0)
                .Subscribe(_ => NextPhase());
        }

        /// <summary>
        /// 次のフェーズへ進める
        /// </summary>
        public void NextPhase()
        {
            phase++;
            if (phase != Phase.GatlingLPhase) _bossTimeline.PlayTimeline(phase);
        }
    }
}