using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 残弾の表示をするクラス
    /// </summary>
    public class GunView : MonoBehaviour
    {
        [SerializeField] private GameObject[] ammo0;     // 残弾表示の1の位
        [SerializeField] private GameObject[] ammo1;     // 残弾表示の10の位
        [SerializeField] private GameObject ammo2;       // 残弾表示の100の位
        [SerializeField] private Animator emptyImage;    // EMPTYの画像
        [SerializeField] private GameObject reloadImage; // RELOADINGの画像
        
        /// <summary>
        /// 配列のidx番目のオブジェクトのactiveをtrueにする
        /// </summary>
        /// <param name="obj">配列</param>
        /// <param name="idx">インデックス</param>
        private void SetValue(GameObject[] obj, int idx)
        {
            for(int i = 0; i < 10; i++) obj[i].SetActive(i == idx);
        }
        
        /// <summary>
        /// 残弾表示の更新
        /// </summary>
        /// <param name="ammo">残弾</param>
        public void ChangeAmmoValue(int ammo)
        {
            ammo2.SetActive(ammo >= 100);
            SetValue(ammo1, (ammo / 10) % 10);
            SetValue(ammo0, ammo % 10);
        }

        /// <summary>
        /// EMPTYを表示
        /// </summary>
        public void DisplayEmptyImage()
        {
            emptyImage.Play("EmptyDisplay");
        }

        /// <summary>
        /// RELOADINGの表示
        /// </summary>
        public void DisplayReloadImage(bool enable)
        {
            reloadImage.SetActive(enable);
        }
    }
}