using Commons.Enum;
using Commons;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace InGame
{
    /// <summary>
    /// インゲーム全体を管理するシングルトンクラス
    /// </summary>
    public class GameManager
    {
        private static GameManager _instance = new GameManager();
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"Error! Please correct!");
                }
                return _instance;
            }
        }
        

        /// <summary>
        /// 現在の状態
        /// </summary>
        private ReactiveProperty<InGameState> _gameState;
        public IReactiveProperty<InGameState> GameState => _gameState;

        /// <summary>
        /// 赤色のゴミの数
        /// </summary>
        private IntReactiveProperty _redTrash;
        public IReactiveProperty<int> RedTrash => _redTrash;

        /// <summary>
        /// 青色のゴミの数
        /// </summary>
        private IntReactiveProperty _blueTrash;
        public IReactiveProperty<int> BlueTrash => _blueTrash;

        /// <summary>
        /// 緑色のゴミの数
        /// </summary>
        private IntReactiveProperty _greenTrash;
        public IReactiveProperty<int> GreenTrash => _greenTrash;

        //リザルトで使用する実体
        //残りのゴミ総数
        private IntReactiveProperty _remainingTrash;
        public IReactiveProperty<int> RemainingTrash => _remainingTrash;

        public int AllTrash;

        //リザルトに表示するデータ群
        //間違って分別してしまったゴミ
        public int FailedTrash;
        //残り時間
        public float RemainingTime;

        //次のステージ番号
        public int CurrentStageNum = 0;

        //ゲーム開始の準備ができたか
        private bool _isReady;

        public bool IsReady { get => _isReady; set => _isReady = value; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private GameManager() 
        {
            Initialize();
        }

        /// <summary>
        /// Initialize(初期化処理)
        /// </summary>
        private void Initialize()
        {
            _gameState = new ReactiveProperty<InGameState>(InGameState.WaitStart);

            _redTrash = new IntReactiveProperty(0);
            _blueTrash = new IntReactiveProperty(0);
            _greenTrash = new IntReactiveProperty(0);

            _remainingTrash = new IntReactiveProperty(0);

            FailedTrash = 0;

            _isReady = false;
        }

        public void SetTrashValue(int value)
        {
            _remainingTrash.Value = value;
        }

        /// <summary>
        /// 状態が変化した際に呼ばれる
        /// </summary>
        private void OnStateChanged(InGameState state)
        {
            switch (state)
            {
                case InGameState.WaitStart:
                    Debug.Log("開始前状態です");
                    break;
                case InGameState.Game:
                    Debug.Log("ゲーム中です");
                    break;
                case InGameState.Finish:
                    Debug.Log("ゲーム終了");
                    SoundManager.Instance.AudioPlay(SoundType.SE, 1);
                    if (PlayerPrefs.GetInt("CLEAR",0)<CurrentStageNum) {
                        PlayerPrefs.SetInt("CLEAR",CurrentStageNum);
                    }
                    AllTrash = RemainingTrash.Value + RedTrash.Value + BlueTrash.Value + GreenTrash.Value + FailedTrash;
                    //リザルトシーンに移動
                    break;
            }
        }

        /// <summary>
        /// スコア加算処理
        /// </summary>
        public void AddScore(TrashType type)
        {
            switch (type)
            {
                case TrashType.Red:
                    _redTrash.Value++;
                    break;

                case TrashType.Bule:
                    _blueTrash.Value++;
                    break;

                case TrashType.Green:
                    _greenTrash.Value++;
                    break;
            }

            _remainingTrash.Value--;

            Clean();
        }

        /// <summary>
        /// 間違って分別してしまった数の加算処理
        /// </summary>
        public void AddFailedTrash()
        {
            FailedTrash++;
            _remainingTrash.Value--;

            Clean();
        }

        private void Clean()
        {
            if (_remainingTrash.Value == 0)
            {
                SetState(InGameState.Finish);
                _isReady = false;
            }
        }

        /// <summary>
        /// ゲームをはじめる
        /// </summary>
        public void GameStart()
        {
            SetState(InGameState.Game);
            _isReady = true;
        }

        /// <summary>
        /// 状態を変える
        /// </summary>
        public void SetState(InGameState state)
        {
            _gameState.Value = state;

            OnStateChanged(state);
        }

        //こいつは次のステージに移動したときに実行
        /// <summary>
        /// ゲームをリスタートさせる
        /// </summary>
        public void GameReset()
        {
            SetState(InGameState.WaitStart);

            _redTrash.Value = 0;
            _blueTrash.Value = 0;
            _greenTrash.Value = 0;
            _remainingTrash.Value = 0;

            FailedTrash = 0;
            RemainingTime = 0;
            AllTrash = 0;

            _isReady = false;

            Debug.Log("リセット処理をしました");
        }
    }
}