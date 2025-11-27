using RpgInventory.Services;

namespace RpgInventory.Models;

// Класс для всех предметов
public abstract class Item
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Description { get; private set; }

    public IUseStrategy UseStrategy { get; private set; }

    public IItemState State { get; private set; }

    protected Item(
        string name,
        string description,
        IUseStrategy useStrategy,
        IItemState state)
    {
        Name = name;
        Description = description;
        UseStrategy = useStrategy;
        State = state;
    }

    public void Use(Player player, Player? target = null)
        => UseStrategy.Use(this, player, target);

    public void Upgrade()
        => State = State.Upgrade();


}