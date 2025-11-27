using RpgInventory.Services;

namespace RpgInventory.Models;

// Класс для оружия
public class Weapon : Item
{
    public int BaseDamage { get; }

    public Weapon(
        string name,
        string description,
        int baseDamage,
        IUseStrategy useStrategy,
        IItemState state)
        : base(name, description, useStrategy, state)
    {
        BaseDamage = baseDamage;
    }

    public int GetDamage() => State.ModifyValue(BaseDamage);
}