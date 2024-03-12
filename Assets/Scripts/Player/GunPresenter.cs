using UnityEngine;
using UniRx;
using Zenject;

namespace Player
{
    public class GunPresenter : MonoBehaviour
    {
        [Inject]
        private GunModel _gunModel;
        [SerializeField]
        private GunView _gunView;

        private void Start()
        {
            // 弾数の表示の変更
            _gunModel
                .AmmoChanged
                .Subscribe(ammo => _gunView.ChangeAmmoValue(ammo));
            
            // EMPTYの表示
            _gunModel
                .AmmoChanged
                .Where(ammo => ammo == 0)
                .Subscribe(_ => _gunView.DisplayEmptyImage());
            
            // RELOADINGの表示
            _gunModel
                .Reloaded
                .Subscribe(isReload => _gunView.DisplayReloadImage(isReload));
        }
    }
}