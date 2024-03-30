using UnityEngine;
using Cysharp.Threading.Tasks;
using Player;
using Zenject;

namespace Boss
{
    /// <summary>
    /// ガトリングで攻撃するクラス
    /// </summary>
    public class GatlingAttack : MonoBehaviour
    {
        [Inject] private PlayerStatus _playerStatus;
        [SerializeField] private BossTimelineController _bossTimelineController;
        [SerializeField] private SEPlayer _sePlayer;
        [SerializeField] private Transform player;
        [SerializeField] private Transform gatlingBaseL;
        [SerializeField] private Transform gatlingBarrelL;
        [SerializeField] private Transform gatlingMuzzleL;
        [SerializeField] private Transform gatlingBaseR;
        [SerializeField] private Transform gatlingBarrelR;
        [SerializeField] private Transform gatlingMuzzleR;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float shotInterval;
        [SerializeField] private float shotDamage;
        
        private float gatlingRotate = 0; // ガトリングの回転量
        
        /// <summary>
        /// ガトリングで攻撃する
        /// </summary>
        /// <param name="length">攻撃の長さ(秒)</param>
        public async void Attack(float length)
        {
            // ガトリングの初期回転
            var startRotateL = gatlingBaseL.rotation;
            var startRotateR = gatlingBaseR.rotation;
            
            // ガトリングをプレイヤーに向けたときの回転
            var armLrotate = Quaternion.LookRotation((player.position + Vector3.up * 1.0f) - gatlingBaseL.transform.position, Vector3.up) * Quaternion.Euler(-90, 0, 0);
            var armRrotate = Quaternion.LookRotation((player.position + Vector3.up * 1.0f) - gatlingBaseR.transform.position, Vector3.up) * Quaternion.Euler(-90, 0, 0);
    
            // タイムラインを一時停止する
            _bossTimelineController.PauseTimeline();
            
            // 1秒かけてガトリングをプレイヤーに向ける
            float time = 0;
            while (time < 1.0f)
            {
                gatlingBaseL.rotation = Quaternion.Lerp(startRotateL, armLrotate, time / 1.0f);
                gatlingBaseR.rotation = Quaternion.Lerp(startRotateR, armRrotate, time / 1.0f);
                time += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
            
            // ガトリングを回転させながら撃つ
            float shotCoolDown = 0;
            time = 0;
            while (time <= length)
            {
                gatlingRotate += Time.deltaTime * rotateSpeed;
                shotCoolDown += Time.deltaTime;
                if (gatlingRotate > 180f) gatlingRotate = -180f;
                gatlingBarrelL.localRotation = Quaternion.Euler(0, -gatlingRotate, 0);
                gatlingBarrelR.localRotation = Quaternion.Euler(0, gatlingRotate, 0);
                
                // 一定間隔で撃つ
                if (shotCoolDown >= shotInterval)
                {
                    shotCoolDown = 0;
                    Shot();
                    _playerStatus.Damage(shotDamage);
                }
                
                time += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
            
            // 1秒かけてガトリングを戻す
            armLrotate = gatlingBaseL.localRotation;
            armRrotate = gatlingBaseR.localRotation;
            time = 0;
            while (time < 1.0f)
            {
                gatlingBaseL.localRotation = Quaternion.Lerp(armLrotate, Quaternion.identity, time / 1.0f);
                gatlingBaseR.localRotation = Quaternion.Lerp(armRrotate, Quaternion.identity, time / 1.0f);
                time += Time.deltaTime;
                await UniTask.DelayFrame(1);
            }
            
            // タイムラインを再開する
            _bossTimelineController.RestartTimeline();
        }
        
        /// <summary>
        /// ガトリングから弾を発射する
        /// </summary>
        private void Shot()
        {
            _sePlayer.PlaySound(4);
            // Instantiate(bullet, muzzleL.transform.position, Quaternion.LookRotation(-muzzleL.transform.up + muzzleL.transform.right * Random.Range(0.01f, 0.03f) + muzzleL.transform.forward * Random.Range(-0.01f, 0.01f)));
            // if (armR[0].gameObject.activeSelf) Instantiate(bullet, muzzleR.transform.position, Quaternion.LookRotation(-muzzleR.transform.up + muzzleR.transform.right * Random.Range(-0.03f, -0.01f) + muzzleR.transform.forward * Random.Range(-0.01f, 0.01f)));
        }
    }
}