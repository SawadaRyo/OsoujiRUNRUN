using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashImageController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler
{
    [Tooltip("�S�~�̎��")]
    [SerializeField]
    private TrashType _trashType = default;

    /// <summary>
    /// ����GameObject��z�u����ꏊ
    /// </summary>
    GameObject _trashListObject = null;
    /// <summary>
    /// ����GameObject�̍��W
    /// </summary>
    RectTransform _rectTransform = default;
    /// <summary>
    /// �������ɔz�u����ꏊ�̃^�O�̖��O
    /// </summary>
    const string TRASH_LIST_TAG_NAME = "TrashList";
    /// <summary>
    /// �S�~���̃^�O�̖��O
    /// </summary>
    const string TRASHBOX_TAG_NAME = "TrashBox";

    private void Start()
    {
        //�������Ɏ�����Holder�ɒǉ�����
        _rectTransform = GetComponent<RectTransform>();
        _trashListObject = GameObject.FindGameObjectWithTag(TRASH_LIST_TAG_NAME);
        transform.SetParent(_trashListObject.transform);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnDisable()
    {
        //��A�N�e�B�u�ɂȂ�����폜����
        Destroy(gameObject);
    }

    /// <summary>
    /// �}�E�X�̈ʒu�ɒǏ]������
    /// </summary>
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        _rectTransform.position = eventData.position;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData){ }

    public void OnPointerUp(PointerEventData eventData)
    {
        var currentDeck = GetCurrentDeck(eventData);

        if (currentDeck)
        {
            //�S�~���Ɏ̂Ă�ꂽ�炻�̏������Ă�
            var trashBoxUIController = FindObjectOfType<TrashBoxUIController>().GetComponent<TrashBoxUIController>();
            trashBoxUIController.DisposeTrash(_trashType);

            //�I�u�W�F�N�g��j��
            Destroy(gameObject);
        }
        else
        {
            //�Ȃɂ��Ȃ��ꏊ�Ƀh���b�O�����猳�̏ꏊ�ɖ߂�
            transform.SetParent(_trashListObject.transform);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //�h���b�O���J�n������Canvas�̎q�̃I�u�W�F�N�g�ɂ���
        transform.SetParent(_trashListObject.transform.parent);
    }

    /// <summary>
    /// �}�E�X���d�Ȃ��Ă���I�u�W�F�N�g���擾����
    /// </summary>
    /// <param name="eventData"></param>
    private GameObject GetCurrentDeck(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        //�}�E�X���d�Ȃ��Ă���I�u�W�F�N�g���܂Ƃ߂�List�Ɋi�[����
        EventSystem.current.RaycastAll(eventData, results);

        RaycastResult result = default;

        //�擾����Object�̒�����S�~�������邩����
        foreach (var r in results)
        {
            if (r.gameObject.CompareTag(TRASHBOX_TAG_NAME))
            {
                result = r;
                break;
            }
        }
        
        return result.gameObject;
    }
}
