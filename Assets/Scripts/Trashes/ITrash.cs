using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public interface ITrash
{
    public TrashType Type { get; }
    public void VacuumGarbage(Transform playerTrans, PlayerContoller player, float interval);
    public bool AbleTrashMater();
}
