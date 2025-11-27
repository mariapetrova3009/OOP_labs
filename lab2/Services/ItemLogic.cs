using RpgInventory.Models;

namespace RpgInventory.Services;

public interface IUseStrategy
{
    void Use(Item item, Player player, Player? target = null);
}

// Оружие атакует цель
public class AttackUseStrategy : IUseStrategy
{
    public void Use(Item item, Player player, Player? target = null)
    {
        if (target == null)
            return;

    if (item is Weapon weapon) {
        var damage = weapon.GetDamage() + player.BaseAttack;
        target.TakeDamage(damage);
        }
    }
}

// Зелье лечит игрока
public class HealUseStrategy : IUseStrategy
{
    public void Use(Item item, Player player, Player? target = null)
    {
        if (item is not Potion potion)
            return;

        player.Heal(potion.GetHealAmount());
    }
}


public class NoUseStrategy : IUseStrategy
{
    public void Use(Item item, Player player, Player? target = null)
    {
    }
}

// Состояние предмета

public interface IItemState
{
    string Name { get; }
    int ModifyValue(int baseValue);
    IItemState Upgrade();
}

// Обычное состояние
public class NormalItemState : IItemState
{
    public string Name => "Normal";

    public int ModifyValue(int baseValue) => baseValue;

    public IItemState Upgrade() => new UpgradedItemState();
}

// Улучшенное состояние
public class UpgradedItemState : IItemState
{
    public string Name => "Upgraded";

    public int ModifyValue(int baseValue) => (int)(baseValue * 1.5);

    public IItemState Upgrade() => this;
}

// Семейства предметов

public interface IItemFactory
{
    Weapon CreateWeapon();
    Armor CreateArmor();
    Potion CreatePotion();
}

// Набор стартовых предметов для воина
public class WarriorItemFactory : IItemFactory
{
    public Weapon CreateWeapon()
    {
        return new Weapon(
            name: "Sword",
            description: "Simple warrior sword",
            baseDamage: 15,
            useStrategy: new AttackUseStrategy(),
            state: new NormalItemState());
    }

    public Armor CreateArmor()
    {
        return new Armor(
            name: "Steel Armor",
            description: "Heavy armor",
            baseDefense: 10,
            useStrategy: new NoUseStrategy(),
            state: new NormalItemState());
    }

    public Potion CreatePotion()
    {
        return new Potion(
            name: "Health Potion",
            description: "Restores health",
            baseHealAmount: 20,
            useStrategy: new HealUseStrategy(),
            state: new NormalItemState());
    }
}

// Набор стартовых предметов для мага
public class MageItemFactory : IItemFactory
{
    public Weapon CreateWeapon()
    {
        return new Weapon(
            name: "Staff",
            description: "Magic staff",
            baseDamage: 10,
            useStrategy: new AttackUseStrategy(),
            state: new NormalItemState());
    }

    public Armor CreateArmor()
    {
        return new Armor(
            name: "Robe",
            description: "Magic robe",
            baseDefense: 5,
            useStrategy: new NoUseStrategy(),
            state: new NormalItemState());
    }

    public Potion CreatePotion()
    {
        return new Potion(
            name: "Mana Potion",
            description: "Restores mana",
            baseHealAmount: 15,
            useStrategy: new HealUseStrategy(),
            state: new NormalItemState());
    }
}