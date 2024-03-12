using System;
using UnityEngine;
using UniRx;

namespace Player
{
    public class UnityInputProvider : IInputProvider
    {
        // トリガー
        private BehaviorSubject<bool> trigger;
        public IObservable<bool> Trigger => trigger;
        
        // リロード
        private BehaviorSubject<bool> reload;
        public IObservable<bool> Reload => reload;
        
        // ハイド
        private BehaviorSubject<bool> hide;
        public IObservable<bool> Hide => hide;
        
        // ポインター
        private BehaviorSubject<Vector3> pointerPos;
        public IObservable<Vector3> PointerPos => pointerPos;

        public UnityInputProvider()
        {
            trigger = new BehaviorSubject<bool>(Input.GetMouseButton(0));
            reload = new BehaviorSubject<bool>(Input.GetKey(KeyCode.R));
            hide = new BehaviorSubject<bool>(Input.GetKey(KeyCode.Space));
            pointerPos = new BehaviorSubject<Vector3>(Input.mousePosition);
        }
    }
}
