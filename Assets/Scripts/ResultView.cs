using System.Linq;
using InGame;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Result
{
    /// <summary>
    /// リザルトの描画を担当するクラス
    /// </summary>
    public class ResultView : MonoBehaviour
    {
        //シーン遷移を発火するボタン
        [SerializeField] private Button _nextStageButton;
        [SerializeField] private Button _stageSelectToButton;
        [SerializeField] private Button _restartToButton;

        [SerializeField] private ParticleSystem _goodEffect;
        [SerializeField] private ParticleSystem _badEffect;

        //残り時間を表示するテキスト
        [SerializeField] private Text _remaingTimeText;
        //分別成功した数を表示するテキスト
        [SerializeField] private Text _correctTrashText;
        //完了度を表示するテキスト
        [SerializeField] private Text _completePercentext;

        [SerializeField] private FadeSystem _fadeSystem;

        //すべてのゴミの数　
        private int _allTrash;
        //残り時間
        private float _remaingTime;
        //分別成功数
        private int _correctTrash;
        //分別失敗数
        private int _failedTrash;
        //完了度
        private int _completePercent;
        //残りの数
        private int _remainingTrash;

        [SerializeField, Header("残り時間がいくつ以上だったらランクを表示する？")] private int _time;
        [SerializeField, Header("間違って分別したゴミがいくつ以下だったらランクを表示する？")] private int _trash;
        [SerializeField, Header("完了度がいくつ以上だったらランクを表示する？")] private int _percent;
        [SerializeField] private Image[] _images;
        [SerializeField] private bool[] _isAward;


        private void Start()
        {
            SoundManager.Instance.AudioPlay(SoundType.BGM, 1);

            Initialize();
            SetEvents();
            LoadData();
            CalcScore();
            ScoreView();

            RankView();


            //このコードは改善したほうが良い
            if (_isAward[0] && _isAward[1] && _isAward[2])
            {
                _goodEffect.Play();
            }
            else
            {
                _badEffect.Play();
            }

            _fadeSystem.StartFadeIn();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialize()
        {
            //とりあえずお試しの
            _isAward = new[] { false, false, false };

            _remaingTimeText.text = "";
            _correctTrashText.text = "";
            _completePercentext.text = "";

            _remaingTime = 0.0f;
            _allTrash = 0;
            _correctTrash = 0;
            _failedTrash = 0;
            _completePercent = 0;
        }

        /// <summary>
        /// SetEvents
        /// </summary>
        private void SetEvents()
        {
            //ボタンのそれぞれのイベントにシーン遷移を結び付ける
            _nextStageButton.onClick.AddListener(() =>
            {
                GameManager.Instance.CurrentStageNum++;
                LoadScene("Stage" + GameManager.Instance.CurrentStageNum.ToString());
            });
            _stageSelectToButton.onClick.AddListener(() =>
            {
                LoadScene("StageSelectScene");
            });
            _restartToButton.onClick.AddListener(() =>
            {
                LoadScene("Stage" + GameManager.Instance.CurrentStageNum.ToString());
            });
        }

        //とりあえずのシーン遷移
        private void LoadScene(string scene)
        {
            _fadeSystem.StartFadeOut(scene);
        }

        /// <summary>
        /// ゲームマネージャのデータを読み込む
        /// </summary>
        private void LoadData()
        {
            _allTrash = GameManager.Instance.AllTrash;
            _remaingTime = GameManager.Instance.RemainingTime;
            _failedTrash = GameManager.Instance.FailedTrash;
            _correctTrash = _allTrash - _failedTrash;
            _remainingTrash = GameManager.Instance.RemainingTrash.Value;

            GameManager.Instance.GameReset();
        }

        /// <summary>
        /// 完了度と総スコアを計算する
        /// </summary>
        private void CalcScore()
        {
            //0を除算するとエラーが出るため確認
            if (_allTrash + _failedTrash == 0) return;
            //_completePercent = 100 - _failedTrash +  _remainingTrash/ (_allTrash + _failedTrash);
            _completePercent = (int)((1f - ((float)_remainingTrash + (float)_failedTrash) / (float)_allTrash) * 100);
        }

        //TODO:ランクのデータを別で持つようにする
        /// <summary>
        /// スコアを表示する
        /// </summary>
        private void ScoreView()
        {
            _remaingTimeText.text = $"{_remaingTime.ToString("F2")}";
            _correctTrashText.text = $"{_correctTrash}個";
            _completePercentext.text = $"{_completePercent}％";
        }

        /// <summary>
        /// ランク（星を描画する）
        /// </summary>
        private void RankView()
        {
            //ここにおくな
            CalRank();

            foreach ((Image image, int index) in _images.Select((x, i) => (x, i)))
            {
                image.gameObject.SetActive(_isAward[index]);
            }

            //スコアを管理するクラスに結果を渡す
            StarScoreManager.Instance.SetStarData(_isAward);
        }

        private void CalRank()
        {
            if (_remaingTime >= _time)
            {
                SetRank(true, 0);
            }
            else
            {
                SetRank(false, 0);
            }


            if (_failedTrash <= _trash)
            {
                SetRank(true, 1);
            }
            else
            {
                SetRank(false, 1);
            }

            if (_completePercent >= _percent)
            {
                SetRank(true, 2);
            }
            else
            {
                SetRank(false, 2);
            }
        }

        /// <summary>
        /// ランク（データ）を設定
        /// </summary>
        private void SetRank(bool isAward, int index)
        {
            _isAward[index] = isAward;
        }
    }
}