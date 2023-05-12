using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TrashBoxController : MonoBehaviour
{
    [Tooltip("���̃S�~���̎��")]
    [SerializeField]
    private TrashType _trashType = default;

    private TrashBoxUIController _uiController = default;

    private const string PLAYER_TAG_NAME = "Player";
    /// <summary>
    /// �S�~�����J�������Ԃ�
    /// </summary>
    public bool _isReadyOpen = false;

    private Transform _tipsTransform = default;

    Sequence _sequence;

    private void Start()
    {
        _uiController = FindObjectOfType<TrashBoxUIController>().GetComponent<TrashBoxUIController>();

        _tipsTransform = transform.GetChild(0);
        _tipsTransform.localScale = new Vector3(0f, 0f, 0f);
    }

    private void Update()
    {
        if (!_isReadyOpen) return;

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E))
        {
            //�S�~�����J��
            _uiController.ShowTrashBox(_trashType);
            //State��ύX
            InGame.GameManager.Instance.GameState.Value = Commons.Enum.InGameState.TrashBoxOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[���ڐG�����珀�������ɂ��ATips���o��
        if (collision.gameObject.CompareTag(PLAYER_TAG_NAME) && !_isReadyOpen)
        {
            _isReadyOpen = true;

            _tipsTransform.localScale = new Vector3(0.3f, 0.3f, 1);
            _sequence = DOTween.Sequence();
            _sequence.Insert(0f, _tipsTransform.DOScale(0.4f, 1f))
                .Insert(1f, _tipsTransform.DOScale(0.3f, 1f))
                .SetLoops(-1)
                .Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG_NAME) && _isReadyOpen)
        {
            _isReadyOpen = false;

            _sequence.Kill();
            _tipsTransform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
