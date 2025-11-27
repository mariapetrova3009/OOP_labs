using System;
using System.Collections.Generic;
using RpgInventory.Models;

namespace RpgInventory.Services;

public class InventoryService
{
    public List<Item> Items { get; } = new List<Item>();

    public void AddItem(Item item)
    {
        if (item == null)
            return;   

        Items.Add(item);
    }

    public bool RemoveItem(Guid id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Id == id)
            {
                Items.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public Item? GetItem(Guid id)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Id == id)
                return Items[i];
        }

        return null;
    }

    public void UseItem(Guid id, Player player, Player? target = null)
    {
        var item = GetItem(id);
        if (item == null)
            return;

        item.Use(player, target);
    }

    public void UpgradeItem(Guid id)
    {
        var item = GetItem(id);
        if (item == null)
            return;

        item.Upgrade();
    }

    // Краткое описание инвентаря
    public string GetSummary()
    {
        if (Items.Count == 0)
            return "Inventory is empty";

        var lines = new List<string>();

        foreach (var item in Items)
        {
            lines.Add($" {item.Name} ({item.GetType().Name}) - {item.State.Name}");
        }

        return string.Join(Environment.NewLine, lines);
    }
}