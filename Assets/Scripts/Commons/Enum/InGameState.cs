namespace Commons.Enum
{
    /// <summary>
    /// 状態
    /// </summary>
    public enum InGameState
    {
        WaitStart, //ゲーム開始前
        TrashBoxOpen, //ゴミ箱を開けた
        Help, //ヘルプ画面を開いている
        Game, //ゲーム中
        Finish, //ゲーム終了
    }
}