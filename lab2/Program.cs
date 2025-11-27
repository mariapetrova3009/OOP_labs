using RpgInventory.Models;
using RpgInventory.Services;


// Создаём игрока
var player = new Player("Hero", maxHealth: 100, baseAttack: 10, baseDefense: 5);


// выводим параметры игрока
Console.WriteLine($"Name: {player.Name}");
Console.WriteLine($"Health: {player.Health}/{player.MaxHealth}");
Console.WriteLine($"Base Attack: {player.BaseAttack}");
Console.WriteLine($"Base Defense: {player.BaseDefense}");
Console.WriteLine();

// Создаём сервисы
var inventory = new InventoryService();
var equipment = new EquipmentService(inventory);

// Абстрактная фабрика — выбираем набор предметов
Console.WriteLine("Creating starter items for Warrior...\n");
IItemFactory factory = new WarriorItemFactory();

// Создаем предметы фабрикой
var sword = factory.CreateWeapon();
var armor = factory.CreateArmor();
var potion = factory.CreatePotion();

// Добавляем предметы в инвентарь
inventory.AddItem(sword);
inventory.AddItem(armor);
inventory.AddItem(potion);

Console.WriteLine("Inventory after adding items:");
Console.WriteLine(inventory.GetSummary());
Console.WriteLine();

// --- Экипировка предметов ---
Console.WriteLine("Equipping sword and armor...");
equipment.EquipWeapon(player, sword.Id);
equipment.EquipArmor(player, armor.Id);

Console.WriteLine($"Player attack: {player.TotalAttack}");
Console.WriteLine($"Player defense: {player.TotalDefense}\n");

// --- Использование предмета ---
Console.WriteLine("Player takes 40 damage...");
player.TakeDamage(40);
Console.WriteLine($"Current HP: {player.Health}");

Console.WriteLine("Using healing potion...");
inventory.UseItem(potion.Id, player);

Console.WriteLine($"Current HP: {player.Health}\n");

// --- Улучшение оружия ---
Console.WriteLine("Upgrading sword...");
inventory.UpgradeItem(sword.Id);

Console.WriteLine("Upgraded inventory:");
Console.WriteLine(inventory.GetSummary());
Console.WriteLine();

Console.WriteLine("Player attack after upgrade:");
Console.WriteLine(player.TotalAttack + "\n");

