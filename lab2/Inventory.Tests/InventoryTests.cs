using System;
using RpgInventory.Models;
using RpgInventory.Services;
using Xunit;

namespace RpgInventory.Tests
{
    // ---- ТЕСТЫ ИГРОКА ----
    public class PlayerTests
    {
        [Fact]
        public void TakeDamage_RespectsDefense_AndDoesNotGoBelowZero()
        {
            var player = new Player("Hero", maxHealth: 100, baseAttack: 10, baseDefense: 5);

            // урона меньше защиты — ХП не меняется
            player.TakeDamage(4);
            Assert.Equal(100, player.Health);

            // 20 - 5 = 15 урона
            player.TakeDamage(20);
            Assert.Equal(85, player.Health);

            // большой урон не уводит здоровье ниже 0
            player.TakeDamage(1000);
            Assert.Equal(0, player.Health);
        }

        [Fact]
        public void Heal_CannotExceedMaxHealth()
        {
            var player = new Player("Hero", maxHealth: 100, baseAttack: 10, baseDefense: 5);

            player.TakeDamage(40); // будет меньше 100
            Assert.True(player.Health < player.MaxHealth);

            player.Heal(1000); // пробуем overheal
            Assert.Equal(100, player.Health);
        }

        [Fact]
        public void TotalAttack_And_TotalDefense_IncludeEquippedItems()
        {
            var player = new Player("Hero", maxHealth: 100, baseAttack: 10, baseDefense: 5);

            // оружие 20 дамага (Normal)
            var weapon = new Weapon(
                name: "Sword",
                description: "",
                baseDamage: 20,
                useStrategy: new AttackUseStrategy(),
                state: new NormalItemState());

            // броня 10 защиты (Normal)
            var armor = new Armor(
                name: "Armor",
                description: "",
                baseDefense: 10,
                useStrategy: new NoUseStrategy(),
                state: new NormalItemState());

            player.EquipWeapon(weapon);
            player.EquipArmor(armor);

            // атака = 10 + 20
            Assert.Equal(30, player.TotalAttack);
            // защита = 5 + 10
            Assert.Equal(15, player.TotalDefense);
        }
    }

    // ---- ТЕСТЫ СОСТОЯНИЙ (STATE) ----
    public class ItemStateTests
    {
        [Fact]
        public void NormalItemState_DoesNotChangeValue()
        {
            var state = new NormalItemState();

            Assert.Equal(10, state.ModifyValue(10));
            Assert.Equal(25, state.ModifyValue(25));
        }

        [Fact]
        public void UpgradedItemState_IncreasesValueBy50Percent()
        {
            var state = new UpgradedItemState();

            Assert.Equal(15, state.ModifyValue(10));   // 10 * 1.5 = 15
            Assert.Equal(30, state.ModifyValue(20));   // 20 * 1.5 = 30
        }

        [Fact]
        public void Upgrade_ChangesNormalToUpgraded_AndKeepsUpgraded()
        {
            IItemState normal = new NormalItemState();
            IItemState upgraded = normal.Upgrade();

            Assert.IsType<UpgradedItemState>(upgraded);

            // дальнейший апгрейд не меняет состояние
            var upgradedAgain = upgraded.Upgrade();
            Assert.Same(upgraded, upgradedAgain);
        }
    }

    // ---- ТЕСТЫ СТРАТЕГИЙ И USE/UPGRADE ЧЕРЕЗ InventoryService ----
    public class InventoryServiceTests
    {
        [Fact]
        public void AddItem_And_GetItem_WorkCorrectly()
        {
            var inventory = new InventoryService();
            var sword = new Weapon(
                "Sword", "",
                baseDamage: 15,
                useStrategy: new AttackUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(sword);

            var found = inventory.GetItem(sword.Id);
            Assert.Same(sword, found);
        }

        [Fact]
        public void UseItem_WithPotion_HealsPlayer()
        {
            var player = new Player("Hero", maxHealth: 100, baseAttack: 10, baseDefense: 5);
            var inventory = new InventoryService();

            var potion = new Potion(
                "Health Potion", "",
                baseHealAmount: 20,
                useStrategy: new HealUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(potion);

            player.TakeDamage(40); // 40 - 5 = 35 урона → 65 HP
            Assert.Equal(65, player.Health);

            inventory.UseItem(potion.Id, player);

            // лечим на 20 → 85
            Assert.Equal(85, player.Health);
        }

        [Fact]
        public void UseItem_WithWeapon_DealsDamageToTarget()
        {
            var attacker = new Player("Attacker", maxHealth: 100, baseAttack: 10, baseDefense: 0);
            var target = new Player("Target", maxHealth: 100, baseAttack: 0, baseDefense: 0);

            var inventory = new InventoryService();

            var weapon = new Weapon(
                "Sword", "",
                baseDamage: 20,
                useStrategy: new AttackUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(weapon);

            inventory.UseItem(weapon.Id, attacker, target);

            // урон = weapon.GetDamage(20) + 10 = 30, защита цели 0 → 70 HP
            Assert.Equal(70, target.Health);
        }

        [Fact]
        public void UpgradeItem_ChangesItemStateToUpgraded()
        {
            var inventory = new InventoryService();

            var weapon = new Weapon(
                "Sword", "",
                baseDamage: 20,
                useStrategy: new AttackUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(weapon);

            inventory.UpgradeItem(weapon.Id);

            Assert.Equal("Upgraded", weapon.State.Name);
            // проверим и урон
            Assert.Equal(30, weapon.GetDamage()); // 20 * 1.5 = 30
        }
    }

    // ---- ТЕСТЫ ЭКИПИРОВКИ ----
    public class EquipmentServiceTests
    {
        [Fact]
        public void EquipWeapon_SetsPlayersEquippedWeapon()
        {
            var player = new Player("Hero", 100, 10, 5);
            var inventory = new InventoryService();
            var equipment = new EquipmentService(inventory);

            var weapon = new Weapon(
                "Sword", "",
                baseDamage: 20,
                useStrategy: new AttackUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(weapon);

            equipment.EquipWeapon(player, weapon.Id);

            Assert.Same(weapon, player.EquippedWeapon);
            Assert.Equal(30, player.TotalAttack); // 10 + 20
        }

        [Fact]
        public void EquipArmor_SetsPlayersEquippedArmor()
        {
            var player = new Player("Hero", 100, 10, 5);
            var inventory = new InventoryService();
            var equipment = new EquipmentService(inventory);

            var armor = new Armor(
                "Armor", "",
                baseDefense: 10,
                useStrategy: new NoUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(armor);

            equipment.EquipArmor(player, armor.Id);

            Assert.Same(armor, player.EquippedArmor);
            Assert.Equal(15, player.TotalDefense); // 5 + 10
        }

        [Fact]
        public void EquipWeapon_WithNonWeaponItem_Throws()
        {
            var player = new Player("Hero", 100, 10, 5);
            var inventory = new InventoryService();
            var equipment = new EquipmentService(inventory);

            var potion = new Potion(
                "Potion", "",
                baseHealAmount: 10,
                useStrategy: new HealUseStrategy(),
                state: new NormalItemState());

            inventory.AddItem(potion);

            Assert.Throws<InvalidOperationException>(
                () => equipment.EquipWeapon(player, potion.Id));
        }
    }

    // ---- ТЕСТЫ FABRIC + BUILDER/CRAFTING ----
    public class FactoryAndBuilderTests
    {
        [Fact]
        public void WarriorFactory_CreatesExpectedWeaponArmorAndPotion()
        {
            IItemFactory factory = new WarriorItemFactory();

            var sword = factory.CreateWeapon();
            var armor = factory.CreateArmor();
            var potion = factory.CreatePotion();

            Assert.Equal("Sword", sword.Name);
            Assert.Equal("Steel Armor", armor.Name);
            Assert.Equal("Health Potion", potion.Name);

            Assert.IsType<AttackUseStrategy>(sword.UseStrategy);
            Assert.IsType<NoUseStrategy>(armor.UseStrategy);
            Assert.IsType<HealUseStrategy>(potion.UseStrategy);

            Assert.Equal("Normal", sword.State.Name);
            Assert.Equal("Normal", armor.State.Name);
            Assert.Equal("Normal", potion.State.Name);
        }

        [Fact]
        public void CraftingService_CreatesUpgradedWeapon_WithCorrectParams()
        {
            var crafting = new CraftingService();

            var weapon = crafting.CreateUpgradedWeapon("Axe of Fury", 20);

            Assert.Equal("Axe of Fury", weapon.Name);
            Assert.Equal(20, weapon.BaseDamage);
            Assert.Equal("Upgraded", weapon.State.Name);

            // урон должен быть увеличен состоянием
            Assert.Equal(30, weapon.GetDamage()); // 20 * 1.5
        }
    }
}