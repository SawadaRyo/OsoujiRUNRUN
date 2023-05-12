using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashImageController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IBeginDragHandler
{
    [Tooltip("ゴミの種類")]
    [SerializeField]
    private TrashType _trashType = default;

    /// <summary>
    /// このGameObjectを配置する場所
    /// </summary>
    GameObject _trashListObject = null;
    /// <summary>
    /// このGameObjectの座標
    /// </summary>
    RectTransform _rectTransform = default;
    /// <summary>
    /// 生成時に配置する場所のタグの名前
    /// </summary>
    const string TRASH_LIST_TAG_NAME = "TrashList";
    /// <summary>
    /// ゴミ箱のタグの名前
    /// </summary>
    const string TRASHBOX_TAG_NAME = "TrashBox";

    private void Start()
    {
        //生成時に自動でHolderに追加する
        _rectTransform = GetComponent<RectTransform>();
        _trashListObject = GameObject.FindGameObjectWithTag(TRASH_LIST_TAG_NAME);
        transform.SetParent(_trashListObject.transform);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void OnDisable()
    {
        //非アクティブになったら削除する
        Destroy(gameObject);
    }

    /// <summary>
    /// マウスの位置に追従させる
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
            //ゴミ箱に捨てられたらその処理を呼ぶ
            var trashBoxUIController = FindObjectOfType<TrashBoxUIController>().GetComponent<TrashBoxUIController>();
            trashBoxUIController.DisposeTrash(_trashType);

            //オブジェクトを破棄
            Destroy(gameObject);
        }
        else
        {
            //なにもない場所にドラッグしたら元の場所に戻す
            transform.SetParent(_trashListObject.transform);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        //ドラッグを開始したらCanvasの子のオブジェクトにする
        transform.SetParent(_trashListObject.transform.parent);
    }

    /// <summary>
    /// マウスが重なっているオブジェクトを取得する
    /// </summary>
    /// <param name="eventData"></param>
    private GameObject GetCurrentDeck(PointerEventData eventData)
    {
        var results = new List<RaycastResult>();
        //マウスが重なっているオブジェクトをまとめてListに格納する
        EventSystem.current.RaycastAll(eventData, results);

        RaycastResult result = default;

        //取得したObjectの中からゴミ箱があるか判定
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
