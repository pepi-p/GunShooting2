using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Player
{
    /// <summary>
    /// プレイヤー周りのUI表示
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        [Inject] private IInputProvider _inputProvider; // 入力
        [SerializeField] private RectTransform reticle; // 照準の座標
        [SerializeField] private RectTransform uiRoot;  // UI全体
        [SerializeField] private Image playerHPbar;     // プレイヤーのHPバー
        [SerializeField] private Image damageImage;     // 被弾時に表示させる画像

        private void Update()
        {
            SetReticlePos(_inputProvider.GetPointerPos());
        }

        /// <summary>
        /// 照準の座標を変更
        /// </summary>
        /// <param name="pos">座標</param>
        private void SetReticlePos(Vector3 pos)
        {
            reticle.position = pos;
        }
    }
}