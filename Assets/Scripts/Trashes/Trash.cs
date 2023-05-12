using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class Trash : MonoBehaviour, ITrash
{
    [SerializeField, Tooltip("吸い込めるようになる時間")]
    float _trashValue = 5f;
    [SerializeField, Tooltip("ごみの種類")]
    TrashType _type = default;
    [SerializeField, Tooltip("埃のRenderer")]
    Renderer _dustRenderer = null;

    [Tooltip("現在の吸い込めるようになる時間")]
    float _courrentTrashValue = 0f;
    [Tooltip("現在の吸い込めるようになる距離")]
    float _courrentTrashDis = 1.6f;

    public TrashType Type => _type;

    private void Start()
    {
        _courrentTrashValue = _trashValue;
    }

    /// <summary>
    /// プレイヤーが吸い込めるかをboolで返す関数
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
    /// プレイヤーに吸い込まれる挙動
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
    /// ゴミの周りの埃のRendererをフェード処理させる関数
    /// </summary>
    /// <param name="parsentOfTrash"></param>
    void FadeInRenderer(float parsentOfTrash)
    {
        Color color = _dustRenderer.material.color;
        color.a = parsentOfTrash;
        _dustRenderer.material.color = color;
    }
}


