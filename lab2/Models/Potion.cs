using RpgInventory.Services;

namespace RpgInventory.Models;

public class Potion : Item
{
    public int BaseHealAmount { get; }

    public Potion(
        string name,
        string description,
        int baseHealAmount,
        IUseStrategy useStrategy,
        IItemState state)
        : base(name, description, useStrategy, state)
    {
        BaseHealAmount = baseHealAmount;
    }

    public int GetHealAmount() => State.ModifyValue(BaseHealAmount);
}