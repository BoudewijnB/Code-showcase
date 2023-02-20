using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    [SerializeField] UI_StatsDisplay uI_StatsDisplay;

    [SerializeField] public PlayerClass playerClass;

    // Start is called before the first frame update
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;

        if (uI_StatsDisplay != null)
        {
            uI_StatsDisplay.UpdateUIStatText(armor, stamina, strength, agility, intelligence, dexterity);
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem, int characterNumber)
    {
        if (newItem.equipSlot == EquipmentSlot.LevelSlot)
        {
            // WARRIOR
            if (characterNumber == 0 && playerClass == PlayerClass.Warrior)
            {
                if (newItem != null)
                {
                    AddStats(newItem);
                }

                if (oldItem != null)
                {
                    RemoveStats(oldItem);
                }
            }
            // WIZARD
            if (characterNumber == 1 && playerClass == PlayerClass.Wizard)
            {
                if (newItem != null)
                {
                    AddStats(newItem);
                }

                if (oldItem != null)
                {
                    RemoveStats(oldItem);
                }
            }
            // ARCHER
            if (characterNumber == 2 && playerClass == PlayerClass.Archer)
            {
                if (newItem != null)
                {
                    AddStats(newItem);
                }

                if (oldItem != null)
                {
                    RemoveStats(oldItem);
                }
            }

            if (uI_StatsDisplay != null)
            {
                uI_StatsDisplay.UpdateUIStatText(armor, stamina, strength, agility, intelligence, dexterity);
            }
        }
    }

    private void AddStats(Equipment item)
	{
        damage.AddModifier(item.damageModifier);

        armor.AddModifier(item.armorModifierArcher);
        stamina.AddModifier(item.staminaModifierArcher);
        strength.AddModifier(item.strengthModifierArcher);
        agility.AddModifier(item.agilityModifierArcher);
        intelligence.AddModifier(item.intelligenceModifierArcher);
        dexterity.AddModifier(item.dexterityModifierArcher);
    }

    private void RemoveStats(Equipment item)
	{
        damage.RemoveModifier(item.damageModifier);

        armor.RemoveModifier(item.armorModifierArcher);
        stamina.RemoveModifier(item.staminaModifierArcher);
        strength.RemoveModifier(item.strengthModifierArcher);
        agility.RemoveModifier(item.agilityModifierArcher);
        intelligence.RemoveModifier(item.intelligenceModifierArcher);
        dexterity.RemoveModifier(item.dexterityModifierArcher);
    }
}

public enum PlayerClass { Warrior, Wizard, Archer }
