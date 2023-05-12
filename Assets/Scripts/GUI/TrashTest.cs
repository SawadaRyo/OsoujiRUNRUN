using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTest : MonoBehaviour
{
    [SerializeField] TrashBoxUIController _t;
    [SerializeField] TrashType _trashType;

    public void Show()
    {
        _t.ShowTrashBox(_trashType);
    }
}
