using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerContoller : MonoBehaviour
{
    [Tooltip("�����ł���S�~�̐�")]
    float _maxGarbageIndex = 5;
    [Tooltip("���ݕێ����Ă���S�~�̃��X�g")]
    List<TrashType> _currentTrashs = new();
    [Tooltip("���ݕێ����Ă���S�~�̐�")]
    IntReactiveProperty _trashCount = new();


    public float maxGarbage => _maxGarbageIndex;
    /// <summary>
    /// ���ݕێ����Ă���S�~�̃��X�g(�ǂݎ���p)
    /// </summary>
    public List<TrashType> CurrentTrashs => _currentTrashs;
    /// <summary>
    /// ���ݕێ����Ă���S�~�̐�(�ǂݎ���p)
    /// </summary>
    public IReadOnlyReactiveProperty<int> TrashCount => _trashCount;

    /// <summary>
    /// �͈͓��̃S�~���z������֐�
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
    /// �w�肳�ꂽ�͈͓��Ō��m�����I�u�W�F�N�g��ITrash�^�̔z��ŕԂ��֐�
    /// </summary>
    /// <returns></returns>
    ITrash[] CheckTrashes(Transform boxVec, Vector2 _boxSize, LayerMask checkTrashLayer)
    {
        //�w�肵���͈͂�Collider�����m
        Collider2D[] objects = Physics2D.OverlapBoxAll((Vector2)boxVec.position, _boxSize, 0f, checkTrashLayer);

        if (objects.Length == 0) return null;

        ITrash[] garbegeObjects = new ITrash[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            //�v�f���Ƃ�Ray���΂��A���̊Ԃɕǂ̃I�u�W�F�N�g���Ȃ���Δz��̗v�f�ɉ�����
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
    /// �z�������S�~��z��ɉ�����֐�
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
    /// �������Ă������̃S�~�̗v�f�������֐�
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
        //4�̒��_�̈ʒu���擾
        Vector2 rightUp = new Vector2(point.x + size.x / 2, point.y + size.y / 2);//�E��
        Vector2 leftUp = new Vector2(point.x - size.x / 2, point.y + size.y / 2);//����
        Vector2 leftDown = new Vector2(point.x - size.x / 2, point.y - size.y / 2);//����
        Vector2 rightDown = new Vector2(point.x + size.x / 2, point.y - size.y / 2);//�E��

        //�`��
        Debug.DrawLine(rightUp, leftUp, color);
        Debug.DrawLine(leftUp, leftDown, color);
        Debug.DrawLine(leftDown, rightDown, color);
        Debug.DrawLine(rightDown, rightUp, color);
    }
#endif
}


