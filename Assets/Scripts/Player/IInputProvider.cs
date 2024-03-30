using UnityEngine;

namespace Player
{
    /// <summary>
    /// 入力のインターフェイス
    /// </summary>
    interface IInputProvider
    {
        public bool GetTrigger();
        public bool GetReload();
        public bool GetHide();
        public Vector3 GetPointerPos();
    }
}
