/// <summary>
/// interface for the various items that the player can use (strategy pattern).
/// </summary>
public interface IItemBehavior
{
    //must implement player data
    void UseItem(PlayerData playerData);
}
