using UnityEngine;
using Zenject;

namespace Boss
{
    public class BossInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // ボスのステータス
            Container
                .Bind<BossStatus>()
                .AsSingle();
            
            // ボスのフェーズ
            Container
                .Bind<BossPhase>()
                .AsSingle();
        }
    }
}