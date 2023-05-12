using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerContoller : MonoBehaviour
{
    [Tooltip("所持できるゴミの数")]
    float _maxGarbageIndex = 5;
    [Tooltip("現在保持しているゴミのリスト")]
    List<TrashType> _currentTrashs = new();
    [Tooltip("現在保持しているゴミの数")]
    IntReactiveProperty _trashCount = new();


    public float maxGarbage => _maxGarbageIndex;
    /// <summary>
    /// 現在保持しているゴミのリスト(読み取り専用)
    /// </summary>
    public List<TrashType> CurrentTrashs => _currentTrashs;
    /// <summary>
    /// 現在保持しているゴミの数(読み取り専用)
    /// </summary>
    public IReadOnlyReactiveProperty<int> TrashCount => _trashCount;

    /// <summary>
    /// 範囲内のゴミを吸引する関数
    /// </summary>
    public void VacuumingGarbege(bool isInput, Transform boxVec, Vector2 boxSize, LayerMask checkTrashLayer)
    {
        if (!isInput) return;

        ITrash[] garbages = CheckTrashes(boxVec, boxSize,checkTrashLayer);
        if (garbages != null)
        {
            AddGarbage(garbages);
            //Debug.Log(_currentTrashs.Count);
        }

    }

    /// <summary>
    /// 指定された範囲内で検知したオブジェクトをITrash型の配列で返す関数
    /// </summary>
    /// <returns></returns>
    ITrash[] CheckTrashes(Transform boxVec, Vector2 _boxSize, LayerMask checkTrashLayer)
    {
        //指定した範囲でColliderを検知
        Collider2D[] objects = Physics2D.OverlapBoxAll((Vector2)boxVec.position, _boxSize, 0f, checkTrashLayer);

        if (objects.Length == 0) return null;

        ITrash[] garbegeObjects = new ITrash[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            //要素ごとにRayを飛ばし、その間に壁のオブジェクトがなければ配列の要素に加える
            Vector2 distance = (objects[i].transform.position - this.transform.position);
            Vector2 direction = distance.normalized;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, distance.magnitude);
            if (!hit.collider.CompareTag("Wall") && objects[i].TryGetComponent<ITrash>(out ITrash trash))
            {
                garbegeObjects[i] = trash;
            }
        }
        return garbegeObjects;
    }
    /// <summary>
    /// 吸引したゴミを配列に加える関数
    /// </summary>
    /// <param name="garbage"></param>
    void AddGarbage(ITrash[] garbages)
    {
        if (_currentTrashs.Count + 1 > _maxGarbageIndex) return;
        foreach (ITrash garbage in garbages)
        {
            if (garbage.AbleTrashMater())
            {
                garbage.VacuumGarbage(this.transform, this, 0.1f);
            }
        }
    }
    /// <summary>
    /// 所持している特定のゴミの要素を消す関数
    /// </summary>
    /// <param name="type"></param>
    public void ChangeGarbage(CollectionCommand command,TrashType type)
    {
        switch(command)
        {
            case CollectionCommand.ADD:
                _currentTrashs.Add(type);
                break;
            case CollectionCommand.REMOVE:
                _currentTrashs.Remove(type);
                break;
        }
        _trashCount.Value = _currentTrashs.Count;
    }

#if UNITY_EDITOR
    void DrawOverlapBox(Vector2 point, Vector2 size, Color color)
    {
        //4つの頂点の位置を取得
        Vector2 rightUp = new Vector2(point.x + size.x / 2, point.y + size.y / 2);//右上
        Vector2 leftUp = new Vector2(point.x - size.x / 2, point.y + size.y / 2);//左上
        Vector2 leftDown = new Vector2(point.x - size.x / 2, point.y - size.y / 2);//左下
        Vector2 rightDown = new Vector2(point.x + size.x / 2, point.y - size.y / 2);//右下

        //描画
        Debug.DrawLine(rightUp, leftUp, color);
        Debug.DrawLine(leftUp, leftDown, color);
        Debug.DrawLine(leftDown, rightDown, color);
        Debug.DrawLine(rightDown, rightUp, color);
    }
#endif
}


