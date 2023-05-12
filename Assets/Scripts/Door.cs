using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    /// <summary>
    /// ドアの挙動を管理するクラス
    /// </summary>
    public class Door : MonoBehaviour
    {
        [SerializeField] TrashType trashType;

        [SerializeField, Header("ゴミの数が何個ある時に破壊する？")]
        private int trashBrokeNumb;

        [SerializeField]
        private Text _text;

        private TrashBoxUIController _trashBox;

        private void Start()
        {
            _trashBox = FindObjectOfType<TrashBoxUIController>();
        }

        private void FixedUpdate()
        {
            int trashNum = 0;
            
            if (trashType == TrashType.Red)
            {
                trashNum = _trashBox.RedTrashBoxNum;
            }
            else if (trashType == TrashType.Bule)
            {
                trashNum = _trashBox.BlueTrashBoxNum;
            }
            else if (trashType == TrashType.Green)
            {
                trashNum = _trashBox.GreenTrashBoxNum;
            }

            SetValue(trashNum);
        }

        private void SetValue(int value)
        {
            if (value == trashBrokeNumb)
            {
                Hide();
            }

            _text.text = (trashBrokeNumb -  value).ToString();
        }

        /// <summary>
        /// 扉を非アクティブにする
        /// </summary>
        private void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}