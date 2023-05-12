using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using InGame;

public class InGameUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _startUI = default;

    [SerializeField]
    private GameObject _InGameUI = default;

    [SerializeField]
    private GameObject _trashBoxUI = default;

    [SerializeField]
    private GameObject _helpUI = default;

    [SerializeField]
    private GameObject _finishImage = default;

    [SerializeField]
    private Text _scoreText = default;

    [SerializeField]
    private FadeSystem _fade = default;

    [SerializeField]
    private TrashBoxUIController _trashBoxUIController = default;

    [SerializeField]
    private ParticleSystem _goodParticle = default;

    [SerializeField]
    private ParticleSystem _badParticle = default;

    private void Start()
    {
        _startUI.SetActive(true);

        _InGameUI.SetActive(false);
        _trashBoxUI.SetActive(false);
        _helpUI.SetActive(false);
        _finishImage.SetActive(false);

        //Stateの変更を監視し、FinishだったらFinishGameを実行
        InGame.GameManager.Instance.GameState
            .Where(s => s == Commons.Enum.InGameState.Finish)
            .Subscribe(_ => StartCoroutine(FinishGame()))
            .AddTo(gameObject);

        //Timerを監視し、Textに反映する
        InGame.GameManager.Instance.RemainingTrash
            .Subscribe(SetScore)
            .AddTo(gameObject);

        //FadeInを実行
        _fade.StartFadeIn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowHelpUI();
        }
    }

    private void SetScore(int value)
    {
        Debug.Log("value" + value);
        _scoreText.text = value.ToString("00");
    }

    /// <summary>
    /// Buttonで実行するゲーム開始処理
    /// </summary>
    public void StartGame()
    {
        HideStartUI();
        ShowGUI();

        GameManager.Instance.GameStart();
    }

    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    private IEnumerator FinishGame()
    {
        _trashBoxUIController.HideTrashBox();
        ShowFinishImage();

        if (GameManager.Instance.RemainingTrash.Value == 0)
        {
            _goodParticle?.Play();
        }
        else
        {
            _badParticle?.Play();
        }

        yield return new WaitForSeconds(4f);

        _fade.StartFadeOut("ResultScene");
    }

    private void HideStartUI()
    {
        _startUI.SetActive(false);
    }

    private void ShowGUI()
    {
        _InGameUI.SetActive(true);
    }

    public void ShowFinishImage()
    {
        _InGameUI.SetActive(false);
        _finishImage.SetActive(true);
    }

    private void ShowHelpUI()
    {
        GameManager.Instance.GameState.Value = Commons.Enum.InGameState.Help;
        _helpUI.SetActive(true);
    }

    public void HideHelpUI()
    {
        _helpUI.SetActive(false);
        GameManager.Instance.GameState.Value = Commons.Enum.InGameState.Game;
    }

    public void Restart()
    {
        GameManager.Instance.GameReset();
        _fade.StartFadeOut("Stage" + GameManager.Instance.CurrentStageNum.ToString());
    }

    public void BackTitle()
    {
        GameManager.Instance.GameReset();
        _fade.StartFadeOut("TitleScene");
    }
}
