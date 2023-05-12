using Commons.Utility;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace Title
{
    /// <summary>
    /// シーン遷移だけのクラス
    /// </summary>
    public class TitleManager : MonoBehaviour
    {
        [SerializeField]
        private FadeSystem _fade;

        [SerializeField]
        private RectTransform _titleLogo;

        private Vector3 _defaultPos = new();

        private IEnumerator Start()
        {
            SoundManager.Instance.AudioPlay(SoundType.BGM, 1);

            _defaultPos = _titleLogo.position;
            _fade.StartFadeIn();

            _titleLogo.position = new Vector3(-900, 0, 0);

            yield return new WaitForSeconds(1.5f);

            _titleLogo.DOMove(_defaultPos, 1f).SetEase(Ease.OutBounce);
        }

        /// <summary>
        /// ステージセレクトシーンに移動
        /// </summary>
        public void OnSelectSceneTo()
        {
            _fade.StartFadeOut("StageSelectScene");
        }

        /// <summary>
        /// ゲームを終了する
        /// </summary>
        public void OnQuitGame()
        {
            Application.Quit();
        }
    }
}