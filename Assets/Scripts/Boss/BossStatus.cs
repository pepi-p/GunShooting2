using System;
using UniRx;
using Zenject;

namespace Boss
{
    /// <summary>
    /// ボスのステータスを管理するクラス
    /// </summary>
    public class BossStatus
    {
        [Inject] private BossPhase _bossPhase;

        private HitPoint[] hpList = new HitPoint[Enum.GetValues(typeof(PartType)).Length];  // パーツごとのHP
        private HitPoint phaseHP;                                                           // 現在のフェーズのHP
        
        // HPの変更を通知
        private BehaviorSubject<HitPoint> phaseHPChanged;
        public IObservable<HitPoint> PhaseHPChanged => phaseHPChanged;

        public BossStatus()
        {
            // HPの初期化
            hpList[(int)PartType.LeftGatling] = new HitPoint(100f);
            hpList[(int)PartType.RightGatling] = new HitPoint(100f);
            hpList[(int)PartType.Body] = new HitPoint(100f);
            phaseHP = hpList[(int)PartType.RightGatling];
            
            phaseHPChanged = new BehaviorSubject<HitPoint>(phaseHP);
        }

        /// <summary>
        /// ボスにダメージを与える
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        public void AddDamage(float damage)
        {
            phaseHP.ReduceHP(damage);
            phaseHPChanged.OnNext(phaseHP);
            
            // フェーズの切り替え
            if (phaseHP.GetHPRate() <= 0)
            {
                _bossPhase.NextPhase();
                
                // パーツの切り替え
                switch (_bossPhase.phase)
                {
                    case Phase.GatlingRPhase:
                        phaseHP = hpList[(int)PartType.LeftGatling];
                        break;
                    case Phase.GatlingLPhase:
                        phaseHP = hpList[(int)PartType.Body];
                        break;
                }
            }
        }
    }
}