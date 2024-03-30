using UnityEngine;

namespace Player
{
    /// <summary>
    /// キーボードとマウスからの入力
    /// </summary>
    public class UnityInputProvider : IInputProvider
    {
        public bool GetTrigger()
        {
            return Input.GetMouseButton(0);
        }

        public bool GetReload()
        {
            return Input.GetKey(KeyCode.R);
        }

        public bool GetHide()
        {
            return Input.GetKey(KeyCode.Space);
        }

        public Vector3 GetPointerPos()
        {
            return Input.mousePosition;
        }
    }
}
