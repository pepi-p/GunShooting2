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
        [SerializeField] private Transform baseArm;
        
        /// <summary>
        /// 右のガトリングを切り離して，力を加える
        /// </summary>
        public void RightGatlingBreak()
        {
            baseArm.gameObject.SetActive(false);
            var rb = gatlingR.GetComponent<Rigidbody>();
            gatlingR.transform.parent = null;
            gatlingR.SetActive(true);
            rb.AddForce(this.transform.right * 20 + Vector3.up * 5, ForceMode.Impulse);

            RightGatlingDestroy();
        }

        /// <summary>
        /// 分離した右ガトリングを削除する
        /// </summary>
        private async void RightGatlingDestroy()
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(5));
            gatlingR.GetComponent<CapsuleCollider>().isTrigger = true;
            await UniTask.Delay(System.TimeSpan.FromSeconds(2));
            gatlingR.SetActive(false);
        }
    }
}