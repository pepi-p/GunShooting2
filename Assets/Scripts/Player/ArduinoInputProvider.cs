using System;
using UnityEngine;
using UniRx;

namespace Player
{
    public class ArduinoInputProvider
    {
        [SerializeField] private Arduino _arduino;
        
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

        public ArduinoInputProvider()
        {
            trigger = new BehaviorSubject<bool>(_arduino.trigger);
            reload = new BehaviorSubject<bool>(_arduino.magazine);
            hide = new BehaviorSubject<bool>(_arduino.hide);
            pointerPos = new BehaviorSubject<Vector3>(GetPointerPos());
        }
        
        public Vector3 GetPointerPos()
        {
            var yaw = _arduino.yaw;
            if (yaw > 180) yaw -= 360;
            float pointerX = Mathf.Atan(yaw * Mathf.Deg2Rad) * Setting.pointerOffset + Screen.width / 2;
            float pointerY = Mathf.Atan(_arduino.pitch * Mathf.Deg2Rad) * Setting.pointerOffset + Screen.height / 2;
            Vector3 pos = new Vector3(pointerX, pointerY, 0);
            return pos;
        }
    }
}