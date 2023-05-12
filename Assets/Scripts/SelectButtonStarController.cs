using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonStarController : MonoBehaviour
{
    [Header("�\�����鐯��Image")]
    [SerializeField]
    private GameObject[] _star = new GameObject[3];

    private int _stageNum = 0;

    private void Start()
    {
        //�I�u�W�F�N�g�̖��O���炻�̔ԍ����擾����
        _stageNum = int.Parse(gameObject.name.Replace("Stage", ""));

        var starScores = StarScoreManager.Instance.StarScores;
        if (!starScores.ContainsKey(_stageNum)) return;

        bool[] award = starScores[_stageNum];

        for (int i = 0; i < 3; i++)
        {
            //�Ή����������A�N�e�B�u�ɂ���
            _star[i].SetActive(award[i]);
        }
    }
}
