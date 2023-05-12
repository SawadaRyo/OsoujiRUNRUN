using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonStarController : MonoBehaviour
{
    [Header("表示する星のImage")]
    [SerializeField]
    private GameObject[] _star = new GameObject[3];

    private int _stageNum = 0;

    private void Start()
    {
        //オブジェクトの名前からその番号を取得する
        _stageNum = int.Parse(gameObject.name.Replace("Stage", ""));

        var starScores = StarScoreManager.Instance.StarScores;
        if (!starScores.ContainsKey(_stageNum)) return;

        bool[] award = starScores[_stageNum];

        for (int i = 0; i < 3; i++)
        {
            //対応した星をアクティブにする
            _star[i].SetActive(award[i]);
        }
    }
}
