using RpgInventory.Services;

namespace RpgInventory.Models;

// Класс для брони
public class Armor : Item
{
    public int BaseDefense { get; }

    public Armor(
        string name,
        string description,
        int baseDefense,
        IUseStrategy useStrategy,
        IItemState state)
        : base(name, description, useStrategy, state)
    {
        BaseDefense = baseDefense;
    }

    public int GetDefense() => State.ModifyValue(BaseDefense);
}