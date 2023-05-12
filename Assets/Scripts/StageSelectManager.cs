using System.Collections.Generic;
using System.Linq;
using InGame;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace StageSelect
{
    /// <summary>
    /// それぞれのボタンにイベント設定処理をするクラス
    /// </summary>
    public class StageSelectManager : MonoBehaviour
    {
        private ReactiveProperty<int> _stageClearNo;
        public IReadOnlyReactiveProperty<int> StageClearNo => _stageClearNo;

        //ステージ選択ボタン配列
        [SerializeField] private List<Button> stageButtons;

        [SerializeField] private FadeSystem _fade;

        private void Start()
        {
            Initialized();

            SoundManager.Instance.AudioPlay(SoundType.BGM, 1);

            _fade.StartFadeIn();
            SetActiveButton(stageButtons.Count);
            
            Bind();
        }

        //TODO:ぜったいにクラスごとに分けた方が良いので、分けたい...
        /// <summary>
        /// Initialize
        /// </summary>
        private void Initialized()
        {
            _stageClearNo = new ReactiveProperty<int>(PlayerPrefs.GetInt("CLEAR", 0));
        }

        /// <summary>
        /// BInd
        /// </summary>
        private void Bind()
        {
            StageClearNo
                .Subscribe(value => SetActiveButton(value)).AddTo(this);
        }

        /// <summary>
        /// それぞれのボタンのイベントにシーン遷移の機能を結び付ける
        /// </summary>
        private void SetActiveButton(int clearStageNo)
        {
            //クリアしているステージ+未クリアステージ+1のボタンまでボタンを押せるようにする
            foreach ((Button stageButton, int index) in stageButtons.Select((x, i) => (x, i)))
            {
                bool isClear = clearStageNo >= index;
                stageButton.interactable = isClear;

                //ボタンにそれぞれのシーンを結び付ける
                stageButton.onClick?.AddListener(() =>
                {
                    GameManager.Instance.CurrentStageNum = index + 1;
                    _fade.StartFadeOut("Stage" + (index + 1).ToString());
                });
            }
        }
    }
}