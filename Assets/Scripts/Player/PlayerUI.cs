using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    /// <summary>
    /// プレイヤー周りのUI表示
    /// </summary>
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform reticle; // 照準の座標
        [SerializeField] private RectTransform uiRoot;  // UI全体
        [SerializeField] private Image playerHPbar;     // プレイヤーのHPバー
        [SerializeField] private Image damageImage;     // 被弾時に表示させる画像

        // FIXME: デバッグ用
        private void Update()
        {
            SetReticlePos(Input.mousePosition);
        }
        
        /// <summary>
        /// 照準の座標を変更
        /// </summary>
        /// <param name="pos">座標</param>
        public void SetReticlePos(Vector3 pos)
        {
            reticle.position = pos;
        }
    }
}