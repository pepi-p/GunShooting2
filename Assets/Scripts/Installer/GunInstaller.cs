using UnityEngine;
using Zenject;

namespace Player
{
    public class GunInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<GunModel>()
                .AsSingle();
        }
    }
}