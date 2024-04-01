using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Boss
{
    /// <summary>
    /// 右のガトリングを破壊するクラス
    /// </summary>
    public class GatlingDestroy : MonoBehaviour
    {
        [SerializeField] private GameObject gatlingR;
        [SerializeField] private GameObject newGatlingR;
        
        /// <summary>
        /// 右のガトリングを切り離して，力を加える
        /// </summary>
        public void RightGatlingBreak()
        {
            gatlingR.gameObject.SetActive(false);
            var rb = newGatlingR.GetComponent<Rigidbody>();
            newGatlingR.transform.parent = null;
            newGatlingR.SetActive(true);
            rb.AddForce(this.transform.right * 20 + Vector3.up * 5, ForceMode.Impulse);

            RightGatlingDestroy();
        }

        /// <summary>
        /// 分離した右ガトリングを削除する
        /// </summary>
        private async void RightGatlingDestroy()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(5));
            newGatlingR.GetComponent<CapsuleCollider>().isTrigger = true;
            await UniTask.Delay(System.TimeSpan.FromSeconds(2));
            newGatlingR.SetActive(false);
        }
    }
}