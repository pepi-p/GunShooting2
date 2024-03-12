using UnityEngine;
using UniRx;
using Zenject;

namespace Player
{
    /// <summary>
    /// 銃のリロードをするクラス
    /// </summary>
    public class Reload : MonoBehaviour
    {
        [Inject]
        private GunModel _gunModel;
        
        private void Start()
        {
            // 押下
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.R))
                .Subscribe(_ => _gunModel.ReloadStart())
                .AddTo(gameObject);
            
            // 押上
            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyUp(KeyCode.R))
                .Subscribe(_ => _gunModel.ReloadEnd())
                .AddTo(gameObject);
        }
    }
}