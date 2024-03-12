using UnityEngine;
using Zenject;
using System.Threading;
using UniRx;
using TimeSpan = System.TimeSpan;

namespace Player
{
    /// <summary>
    /// 入力から動作を呼ぶ
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerUI _playerUI;
        [SerializeField]
        private Raycast _raycast;
        [Inject]
        private GunModel _playerGunModel;
        [Inject]
        private IInputProvider _inputProvider;
        
        private bool easyReload; // 隠れるだけでリロードするのを許可
        private bool isReload;   // リロード中か

        private void Update()
        {
            /*
            bool trigger = _inputProvider.GetTrigger();
            bool reload = _inputProvider.GetReload();
            bool hide = _inputProvider.GetHide();
            Vector3 pointerPos = _inputProvider.GetPointerPos();
            
            _playerUI.SetReticlePos(pointerPos);
            
            // easyReloadが有効なときはhideで，無効なときはマガジンを外すことでリロード
            if ((!easyReload && reload) || (easyReload && hide))
            {
                isReload = true;
                // StartCoroutine(MortorSetEnable(false));
                _playerGunModel.ReloadStart();
            }
            // マガジンが再度装着されたらリロード終了
            else if (isReload)
            {
                isReload = false;
                _playerGunModel.ReloadEnd();
                // StartCoroutine(MortorSetEnable(true));
                // serialHandler.Write("100\n");
            }
            */
        }

        private void Shot()
        {
            
            /*
            var hit = _raycast.PointerRay();
            if (hit.collider != null)
            {
                
            }
            */
        }
    }
}