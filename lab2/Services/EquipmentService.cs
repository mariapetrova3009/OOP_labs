using RpgInventory.Models;

namespace RpgInventory.Services;

public class EquipmentService
{
    private InventoryService _inventory;

    public EquipmentService(InventoryService inventory)
    {
        _inventory = inventory;
    }

    public void EquipWeapon(Player player, Guid itemId)
    {
        var item = _inventory.GetItem(itemId);
        if (item == null)
            return; 

        if (item is Weapon weapon)
        {
            player.EquipWeapon(weapon);
        }
    }

    public void EquipArmor(Player player, Guid itemId)
    {
        var item = _inventory.GetItem(itemId);
        if (item == null)
            return;

        if (item is Armor armor)
        {
            player.EquipArmor(armor);
        }
    }
}