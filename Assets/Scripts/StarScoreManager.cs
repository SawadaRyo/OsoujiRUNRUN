using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

/// <summary>
/// �N���A�����X�e�[�W�̃X�R�A�̏�����������V���O���g��
/// </summary>
public class StarScoreManager
{
    private static StarScoreManager _instance = new StarScoreManager();
    public static StarScoreManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"Error! Please correct!");
            }
            return _instance;
        }
    }

    private Dictionary<int, bool[]> _starScores = new();
    public Dictionary<int, bool[]> StarScores => _starScores;

    public void SetStarData(bool[] isAward)
    {
        int stageNum = GameManager.Instance.CurrentStageNum;

        //Key�̏d�����N���Ȃ��悤�ɗv�f���`�F�b�N
        if (!_starScores.ContainsKey(stageNum))
        {
            _starScores.Add(stageNum,isAward);
        }
    }
}
