namespace RpgInventory.Models;

public class Player
{
    public string Name { get; }

    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public int BaseAttack { get; private set; }
    public int BaseDefense { get; private set; }

    public Weapon? EquippedWeapon { get; private set; }
    public Armor? EquippedArmor { get; private set; }

    public int TotalAttack {
    get {
        if (EquippedWeapon == null)
            return BaseAttack;

        return BaseAttack + EquippedWeapon.GetDamage();
        }
    }
    public int TotalDefense {
    get{
        if (EquippedArmor == null)
            return BaseDefense;

        return BaseDefense + EquippedArmor.GetDefense();
        }
    }

    public Player(string name, int maxHealth = 100, int baseAttack = 10, int baseDefense = 5)
    {
        Name = name;
        MaxHealth = maxHealth;
        Health = maxHealth;
        BaseAttack = baseAttack;
        BaseDefense = baseDefense;
    }
    
    public void TakeDamage(int amount)
    {
        var damage = Math.Max(0, amount - TotalDefense);
        Health = Math.Max(0, Health - damage);
    }

    public void Heal(int amount)
    {
        Health = Math.Min(MaxHealth, Health + amount);
    }

    public void EquipWeapon(Weapon weapon) => EquippedWeapon = weapon;

    public void EquipArmor(Armor armor) => EquippedArmor = armor;
}