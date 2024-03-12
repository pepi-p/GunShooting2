using System;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Raycast : MonoBehaviour
    {
        [Inject]
        private IInputProvider _inputProvider;

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
        }

        /// <summary>
        /// 照準からRaycastを飛ばす
        /// </summary>
        /// <returns>RaycastHit</returns>
        public RaycastHit PointerRay()
        {
            // FIXME: デバッグ用
            // Physics.Raycast(cam.ScreenPointToRay(_inputProvider.GetPointerPos()), out var hit, 50f, 1);
            Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit, 50f, 1);
            return hit;
        }
    }
}