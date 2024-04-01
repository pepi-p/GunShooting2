using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UniRx;
using Zenject;

namespace Boss
{
    /// <summary>
    /// ボスのタイムラインを制御するクラス
    /// </summary>
    public class BossTimelineController : MonoBehaviour
    {
        [SerializeField] private PlayableDirector director;
        [SerializeField] private TimelineAsset[] timelineAssets;

        /// <summary>
        /// タイムラインを再生する
        /// </summary>
        /// <param name="phase">再生するフェーズ</param>
        public void PlayTimeline(Phase phase)
        {
            director.playableAsset = timelineAssets[(int)phase];
            director.time = 0;
            director.Play();
        }

        /// <summary>
        /// タイムラインを一時停止する
        /// </summary>
        public void PauseTimeline()
        {
            director.Pause();
        }

        /// <summary>
        /// タイムラインを停止する
        /// </summary>
        public void RestartTimeline()
        {
            director.Play();
        }
        
        /// <summary>
        /// タイムラインの再生位置を変更
        /// NOTE: タイムラインから呼ぶ
        /// </summary>
        /// <param name="frame">再生位置のフレーム</param>
        public void TimelineLoop(float frame)
        {
            director.time = frame / 60f;
        }
    }
}