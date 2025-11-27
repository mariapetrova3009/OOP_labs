using RpgInventory.Services;

namespace RpgInventory.Models;

public class QuestItem : Item
{
    public QuestItem(
        string name,
        string description,
        IUseStrategy useStrategy,
        IItemState state)
        : base(name, description, useStrategy, state)
    {
    }
}