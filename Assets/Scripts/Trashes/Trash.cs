using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class Trash : MonoBehaviour, ITrash
{
    [SerializeField, Tooltip("�z�����߂�悤�ɂȂ鎞��")]
    float _trashValue = 5f;
    [SerializeField, Tooltip("���݂̎��")]
    TrashType _type = default;
    [SerializeField, Tooltip("����Renderer")]
    Renderer _dustRenderer = null;

    [Tooltip("���݂̋z�����߂�悤�ɂȂ鎞��")]
    float _courrentTrashValue = 0f;
    [Tooltip("���݂̋z�����߂�悤�ɂȂ鋗��")]
    float _courrentTrashDis = 1.6f;

    public TrashType Type => _type;

    private void Start()
    {
        _courrentTrashValue = _trashValue;
    }

    /// <summary>
    /// �v���C���[���z�����߂邩��bool�ŕԂ��֐�
    /// </summary>
    /// <returns></returns>
    public bool AbleTrashMater()
    {
        _courrentTrashValue -= Time.deltaTime;
        float parsentOfTrash = _courrentTrashValue / _trashValue;
        FadeInRenderer(parsentOfTrash);
        if (_courrentTrashValue <= 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// �v���C���[�ɋz�����܂�鋓��
    /// </summary>
    /// <param name="playerTrans"></param>
    public void VacuumGarbage(Transform playerTrans, PlayerContoller player, float interval)
    {
        this.transform.position = Vector3.Lerp(this.transform.position, playerTrans.position, interval);
        if ((playerTrans.position - this.transform.position).magnitude <= _courrentTrashDis)
        {
            SoundManager.Instance.AudioPlay(SoundType.SE, 4);
            player.ChangeGarbage(CollectionCommand.ADD,_type);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// �S�~�̎���̚���Renderer���t�F�[�h����������֐�
    /// </summary>
    /// <param name="parsentOfTrash"></param>
    void FadeInRenderer(float parsentOfTrash)
    {
        Color color = _dustRenderer.material.color;
        color.a = parsentOfTrash;
        _dustRenderer.material.color = color;
    }
}


