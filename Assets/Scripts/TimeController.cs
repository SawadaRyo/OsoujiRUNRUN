using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    /// <summary>
    /// インゲーム内の時間を管理するクラス
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        /// <summary>
        /// タイム
        /// </summary>
        public FloatReactiveProperty Timer;

        [SerializeField, Header("Timerの初期化数値")] private float _initializeTime;

        //タイムを表示するテキスト
        [SerializeField] private Text _text;

        /// <summary>
        /// Initializes
        /// </summary>
        private void Start()
        {
            Timer = new FloatReactiveProperty(_initializeTime);
        }

        /// <summary>
        /// テキストを最新のタイムに更新
        /// </summary>
        private void Update()
        {
            if (GameManager.Instance.IsReady)
            {
                if (IsFinish())
                {
                    GameManager.Instance.SetState(Commons.Enum.InGameState.Finish);
                    GameManager.Instance.IsReady = false;
                }

                SubTime(Time.deltaTime);
            }

            var t = Mathf.Floor(Timer.Value);
            _text.text = $"{t.ToString() + ":" + ((Timer.Value - t) * 100).ToString("00")}";
        }

        /// <summary>
        /// タイムを減算
        /// </summary>
        public void SubTime(float deltaTime)
        {
            var value = Mathf.Clamp(Timer.Value - deltaTime, 0.0f, 60.0f);
            Timer.Value = value;

            GameManager.Instance.RemainingTime = Timer.Value;
        }

        /// <summary>
        /// タイムが0になったらゲーム終了判定を返す
        /// </summary>
        private bool IsFinish()
        {
            if (Timer.Value == 0.0f)
                return true;
            else
                return false;
        }
    }
}