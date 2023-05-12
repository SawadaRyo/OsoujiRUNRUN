using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBoxUIController : MonoBehaviour
{
    [Tooltip("UI��\������")]
    [SerializeField]
    private GameObject _trashBoxUI = default;

    [Tooltip("PlayerController�̃S�~��List���Q�Ƃ��邽�ߎ擾")]
    [SerializeField]
    private PlayerContoller _playerController = default;

    [Tooltip("��������Image�FRed")]
    [SerializeField]
    private GameObject _redTrash = default;

    [Tooltip("��������Image�FBlue")]
    [SerializeField]
    private GameObject _blueTrash = default;

    [Tooltip("��������Image�FGreen")]
    [SerializeField]
    private GameObject _greenTrash = default;

    [Tooltip("��������Image�FNoTrash")]
    [SerializeField]
    private GameObject _noTrash = default;

    [SerializeField]
    private Animator _anim;

    /// <summary>
    /// ���݊J����Ă���S�~���̎��
    /// </summary>
    private TrashType _currentTrashType = default;

    private int _redTrashBoxNum = 0;
    private int _blueTrashBoxNum = 0;
    private int _greenTrashBoxNum = 0;

    public int RedTrashBoxNum => _redTrashBoxNum;
    public int BlueTrashBoxNum => _blueTrashBoxNum;
    public int GreenTrashBoxNum => _greenTrashBoxNum;

    /// <summary>
    /// TrashBoxUI���o���A�S�~��ǉ�
    /// </summary>
    public void ShowTrashBox(TrashType trashType)
    {
        if (InGame.GameManager.Instance.GameState.Value == Commons.Enum.InGameState.TrashBoxOpen) return;

        _currentTrashType = trashType;
        _trashBoxUI.SetActive(true);

        //��ނɉ������S�~�𐶐�����
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
    /// TrashBoxUI���B��
    /// </summary>
    public void HideTrashBox()
    {
        _trashBoxUI.SetActive(false);
        InGame.GameManager.Instance.GameState.Value = Commons.Enum.InGameState.Game;
    }

    /// <summary>
    /// ���݂��̂Ă��ۂ̏���
    /// </summary>
    public void DisposeTrash(TrashType trashType)
    {
        SoundManager.Instance.AudioPlay(SoundType.SE, 4);

        if (trashType == _currentTrashType)
        {
            //GameManager�̃X�R�A���Z�������Ăт���
            InGame.GameManager.Instance.AddScore(trashType);
            _anim.SetTrigger("star");
        }
        else
        {
            //GameManager�̊Ԉ�����񐔂̉��Z�������Ăт���
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

        //PlayerController�̃��X�g����폜����
        _playerController.ChangeGarbage(CollectionCommand.REMOVE,trashType);
    }
}
