using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 入力のインターフェイス
    /// </summary>
    interface IInputProvider
    {
        IObservable<bool> Trigger { get; }
        IObservable<bool> Reload { get; }
        IObservable<bool> Hide { get; }
        IObservable<Vector3> PointerPos { get; }
    }
}
