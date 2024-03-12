using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IInputProvider>()
                .To<UnityInputProvider>()
                .AsSingle();
        }
    }
}