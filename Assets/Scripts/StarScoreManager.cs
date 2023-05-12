using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;

/// <summary>
/// クリアしたステージのスコアの情報を所持するシングルトン
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

        //Keyの重複が起きないように要素をチェック
        if (!_starScores.ContainsKey(stageNum))
        {
            _starScores.Add(stageNum,isAward);
        }
    }
}
