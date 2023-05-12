using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBoxUIController : MonoBehaviour
{
    [Tooltip("UIを表示する")]
    [SerializeField]
    private GameObject _trashBoxUI = default;

    [Tooltip("PlayerControllerのゴミのListを参照するため取得")]
    [SerializeField]
    private PlayerContoller _playerController = default;

    [Tooltip("生成するImage：Red")]
    [SerializeField]
    private GameObject _redTrash = default;

    [Tooltip("生成するImage：Blue")]
    [SerializeField]
    private GameObject _blueTrash = default;

    [Tooltip("生成するImage：Green")]
    [SerializeField]
    private GameObject _greenTrash = default;

    [Tooltip("生成するImage：NoTrash")]
    [SerializeField]
    private GameObject _noTrash = default;

    [SerializeField]
    private Animator _anim;

    /// <summary>
    /// 現在開かれているゴミ箱の種類
    /// </summary>
    private TrashType _currentTrashType = default;

    private int _redTrashBoxNum = 0;
    private int _blueTrashBoxNum = 0;
    private int _greenTrashBoxNum = 0;

    public int RedTrashBoxNum => _redTrashBoxNum;
    public int BlueTrashBoxNum => _blueTrashBoxNum;
    public int GreenTrashBoxNum => _greenTrashBoxNum;

    /// <summary>
    /// TrashBoxUIを出し、ゴミを追加
    /// </summary>
    public void ShowTrashBox(TrashType trashType)
    {
        if (InGame.GameManager.Instance.GameState.Value == Commons.Enum.InGameState.TrashBoxOpen) return;

        _currentTrashType = trashType;
        _trashBoxUI.SetActive(true);

        //種類に応じたゴミを生成する
        foreach (var trash in _playerController.CurrentTrashs)
        {
            GameObject trashObj = default;
            switch (trash)
            {
                case TrashType.Red:
                    trashObj = _redTrash;
                    break;
                case TrashType.Bule:
                    trashObj = _blueTrash;
                    break;
                case TrashType.Green:
                    trashObj = _greenTrash;
                    break;
                case TrashType.NoTrash:
                    trashObj = _noTrash;
                    break;
            }

            Instantiate(trashObj);
        }
    }

    /// <summary>
    /// TrashBoxUIを隠す
    /// </summary>
    public void HideTrashBox()
    {
        _trashBoxUI.SetActive(false);
        InGame.GameManager.Instance.GameState.Value = Commons.Enum.InGameState.Game;
    }

    /// <summary>
    /// ごみを捨てた際の処理
    /// </summary>
    public void DisposeTrash(TrashType trashType)
    {
        SoundManager.Instance.AudioPlay(SoundType.SE, 4);

        if (trashType == _currentTrashType)
        {
            //GameManagerのスコア加算処理を呼びだす
            InGame.GameManager.Instance.AddScore(trashType);
            _anim.SetTrigger("star");
        }
        else
        {
            //GameManagerの間違った回数の加算処理を呼びだす
            InGame.GameManager.Instance.AddFailedTrash();
        }

        switch (_currentTrashType)
        {
            case TrashType.Red:
                _redTrashBoxNum++;
                break;
            case TrashType.Bule:
                _blueTrashBoxNum++;
                break;
            case TrashType.Green:
                _greenTrashBoxNum++;
                break;
            case TrashType.NoTrash:
                
                break;
        }

        //PlayerControllerのリストから削除する
        _playerController.ChangeGarbage(CollectionCommand.REMOVE,trashType);
    }
}
