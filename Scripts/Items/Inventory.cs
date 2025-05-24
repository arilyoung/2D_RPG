using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    //列表用来按顺序存储所有物品，以便在UI中按顺序显示每个物品的槽位。
    public List<InventoryItem> inventoryMaterial;
    public List<InventoryItem> inventoryEquipment;
    public List<InventoryItem> inventory;
    //字典的作用是通过ItemData作为键，快速找到对应的InventoryItem实例，从而管理堆叠数量。
    public Dictionary<ItemData, InventoryItem> inventoryMaterialDictionary;
    public Dictionary<ItemData, InventoryItem> inventoryEquipmentDictionary;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemEquipmentData, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;

    private UIItemSlot[] inventoryItemSlots;
    private UIEquipmentSlot[] equipmentSlot;
    private void Awake()
    {
        //检查是否有已经存在的Inventory实例
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryMaterial = new List<InventoryItem>();
        inventoryEquipment = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryMaterialDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryEquipmentDictionary = new Dictionary<ItemData, InventoryItem>();
        inventory = inventoryEquipment.Concat(inventoryMaterial).ToList();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemEquipmentData, InventoryItem>();

        //获取所有子物体中的UIItemSlot组件
        inventoryItemSlots = inventorySlotsParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotsParent.GetComponentsInChildren<UIEquipmentSlot>();
    }

    // ?
    public void EquiptItem(ItemData _item)
    {
        ItemEquipmentData newEquipment = _item as ItemEquipmentData;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemEquipmentData oldEquipment = null;

        foreach (KeyValuePair<ItemEquipmentData, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnEquipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);

        //将装备的属性添加到角色身上
        newEquipment.AddModifiers();

        //装备时，将物品从背包中移除
        RemoveItem(_item);
    }

    public void UnEquipItem(ItemEquipmentData oldEquipment)
    {
        if(oldEquipment == null)
            return;
            
        if (equipmentDictionary.TryGetValue(oldEquipment, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(oldEquipment);
            //将装备的属性从角色身上移除
            oldEquipment.RemoveModifiers();
        }
    }

    //更新UI
    private void UpdateSlotsUI()
    {
        for(int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemEquipmentData, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].equipmentType)
                    equipmentSlot[i].UpdateSlots(item.Value);
            }
        }

        for(int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].ClearUISlot();
        }

        for (int i = 0; i < inventoryEquipment.Count; i++)
            inventoryItemSlots[i].UpdateSlots(inventoryEquipment[i]);
        for(int i = 0; i < inventoryMaterial.Count; i++)
            inventoryItemSlots[inventoryEquipment.Count + i].UpdateSlots(inventoryMaterial[i]);
    }

    public void AddItem(ItemData _item)
    {
        AddToInventory(_item);

        UpdateSlotsUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (_item == null)
            return;

        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            //如果已经有这个物品，则增加数量
            value.AddToStack();
        }
        else
        {
            //创建新的InventoryItem
            InventoryItem newItem = new InventoryItem(_item);
            if (_item.itemType == ItemType.Material)
            {
                inventoryMaterial.Add(newItem);
                inventoryMaterialDictionary.Add(_item, newItem);
            }
            else if (_item.itemType == ItemType.Equipment)
            {
                inventoryEquipment.Add(newItem);
                inventoryEquipmentDictionary.Add(_item, newItem);
            }
            inventory = inventoryEquipment.Concat(inventoryMaterial).ToList();
            inventoryDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            RemoveFormAddBoth(_item);
            if (inventoryEquipmentDictionary.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackCount <= 1)
                {
                    inventoryEquipment.Remove(value);
                    inventoryEquipmentDictionary.Remove(_item);
                }
                else
                    value.RemoveFromStack();
            }
        }
        else if (_item.itemType == ItemType.Material)
        {
            RemoveFormAddBoth(_item);
            if (inventoryMaterialDictionary.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackCount <= 1)
                {
                    inventoryMaterial.Remove(value);
                    inventoryMaterialDictionary.Remove(_item);
                }
                else
                    value.RemoveFromStack();
            }
        }
        UpdateSlotsUI();
    }

    private void RemoveFormAddBoth(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem valueForBoth))
        {
            if (valueForBoth.stackCount <= 1)
            {
                inventory.Remove(valueForBoth);
                inventoryDictionary.Remove(_item);
            }
            else
                valueForBoth.RemoveFromStack();
        }
    }
}


