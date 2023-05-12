/// <summary>
/// ゴミの種類
/// </summary>
public enum TrashType
{
    Red = 0,   //燃えるゴミ
    Bule = 1,   //燃えないゴミ
    Green = 2,   //粗大ゴミ
    NoTrash = 3   //それ以外
}

public enum CollectionCommand
{
    ADD,
    REMOVE,
}
