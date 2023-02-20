using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public Equipment[] currentEquipment { get; private set; }
    public Equipment[] warriorCurrentEquipment { get; private set; }
    public Equipment[] wizardCurrentEquipment { get; private set; }
    public Equipment[] archerCurrentEquipment { get; private set; }

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem, int characterNumber);
    public OnEquipmentChanged onEquipmentChanged;

    Inventory inventory;

    [SerializeField] GameObject[] characterEquipmentParents;
    [SerializeField] GameObject[] charactersStats; // currently unused

    [SerializeField] GameObject equipmentWindow;

    private void Start()
    {
        inventory = Inventory.instance;

        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        warriorCurrentEquipment = new Equipment[numSlots];
        wizardCurrentEquipment = new Equipment[numSlots];
        archerCurrentEquipment = new Equipment[numSlots];
    }

    // Equip a new item
    public void Equip(int characterNumber, Equipment newItem, int crystalNumber, UI_ItemSlot uI_ItemSlot = null)
    {
        // add icon to equip slot
        if (uI_ItemSlot != null)
        {
            uI_ItemSlot.icon.sprite = newItem.icon;
            uI_ItemSlot.icon.enabled = true;
        }

        if (characterNumber == 0)
        {
            currentEquipment = warriorCurrentEquipment;
        }
        else if (characterNumber == 1)
        {
            currentEquipment = wizardCurrentEquipment;
        }
        else if (characterNumber == 2)
        {
            currentEquipment = archerCurrentEquipment;
        }

        // remove the crystal from the inventory that's dragged onto the equipment slot
        inventory.Remove(Inventory.instance.crystals[crystalNumber]);

        // Find out what slot the item fits in
        int slotIndex = (int)newItem.equipSlot;

        Equipment oldItem = null;

        // If there was already an item in the slot make sure to put it back in the inventory
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        // An item has been equipped so we trigger the callback
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem, characterNumber);
        }

        currentEquipment[slotIndex] = newItem;

        // Unequip other skill crystals
        if (slotIndex == 1)
        {
            Unequip(characterNumber, 2);
            Unequip(characterNumber, 3);
        }
        else if (slotIndex == 2)
        {
            Unequip(characterNumber, 1);
            Unequip(characterNumber, 3);
        }
        else if (slotIndex == 3)
        {
            Unequip(characterNumber, 1);
            Unequip(characterNumber, 2);
        }
    }

    // Unequip an item with a particular index
    public void Unequip(int characterNumber, int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            // remove UI icon from skill slot
            int slotIndexMinusOne = slotIndex - 1; // use slotindex -1 >> first slotIndex for skill slots is 1, 0 is statSlot
            UI_ItemSlot unequipItemSlot = characterEquipmentParents[characterNumber].transform.GetChild(slotIndexMinusOne).GetComponent<UI_ItemSlot>();
            unequipItemSlot.icon.sprite = null;
            unequipItemSlot.icon.enabled = false;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem, characterNumber);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(0, i);
            Unequip(1, i);
            Unequip(2, i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

    public void OpenCloseEquipmentWindow()
    {
        equipmentWindow.SetActive(!equipmentWindow.activeInHierarchy);
    }
}