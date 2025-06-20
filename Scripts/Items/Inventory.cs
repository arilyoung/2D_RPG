using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    //�б�������˳��洢������Ʒ���Ա���UI�а�˳����ʾÿ����Ʒ�Ĳ�λ��
    public List<InventoryItem> inventoryMaterial;
    public List<InventoryItem> inventoryEquipment;
    public List<InventoryItem> inventoryPotion;
    public List<InventoryItem> inventory;
    //�ֵ��������ͨ��ItemData��Ϊ���������ҵ���Ӧ��InventoryItemʵ�����Ӷ�����ѵ�������
    public Dictionary<ItemData, InventoryItem> inventoryMaterialDictionary;
    public Dictionary<ItemData, InventoryItem> inventoryEquipmentDictionary;
    public Dictionary<ItemData, InventoryItem> inventoryPotionDictionary;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemEquipmentData, InventoryItem> equipmentDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotsParent;
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private Transform potionSlotsParent;

    private UIItemSlot[] inventoryItemSlots;
    private UIEquipmentSlot[] equipmentSlot;
    private UIPotionSlot[] potionSlot;
    private void Awake()
    {
        //����Ƿ����Ѿ����ڵ�Inventoryʵ��
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryMaterial = new List<InventoryItem>();
        inventoryEquipment = new List<InventoryItem>();
        inventoryPotion = new List<InventoryItem>();

        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryMaterialDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryEquipmentDictionary = new Dictionary<ItemData, InventoryItem>();
        inventoryPotionDictionary = new Dictionary<ItemData, InventoryItem>();

        inventory = inventoryEquipment.Concat(inventoryMaterial).ToList().Concat(inventoryPotion).ToList();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemEquipmentData, InventoryItem>();

        //��ȡ�����������е�UIItemSlot���
        inventoryItemSlots = inventorySlotsParent.GetComponentsInChildren<UIItemSlot>();
        equipmentSlot = equipmentSlotsParent.GetComponentsInChildren<UIEquipmentSlot>();
        potionSlot = potionSlotsParent.GetComponentsInChildren<UIPotionSlot>();
        Debug.Log(potionSlot.Length);
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

        //��װ����������ӵ���ɫ����
        newEquipment.AddModifiers();

        //װ��ʱ������Ʒ�ӱ������Ƴ�
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
            //��װ�������Դӽ�ɫ�����Ƴ�
            oldEquipment.RemoveModifiers();
        }
    }

    public void ConsumePotion(InventoryItem _item)
    {
        RemoveItemByOne(_item);
        Debug.Log(_item.data.itemType);
    }

    //����UI
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

        for(int i = 0; i < potionSlot.Length; i++)
        {
            potionSlot[i].ClearUISlot();
        }

        for(int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].ClearUISlot();
        }

        for (int i = 0; i < inventoryEquipment.Count; i++)
            inventoryItemSlots[i].UpdateSlots(inventoryEquipment[i]);
        for(int i = 0; i < inventoryMaterial.Count; i++)
            inventoryItemSlots[inventoryEquipment.Count + i].UpdateSlots(inventoryMaterial[i]);
        for (int i = 0; i < inventoryPotion.Count; i++)
            potionSlot[i].UpdateSlots(inventoryPotion[i]);
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
            //����Ѿ��������Ʒ������������
            value.AddToStack();
        }
        else
        {
            //�����µ�InventoryItem
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
            else if (_item.itemType == ItemType.Potion)
            {
                inventoryPotion.Add(newItem);
                inventoryPotionDictionary.Add(_item, newItem);
            }
            inventory = inventoryEquipment.Concat(inventoryMaterial).ToList().Concat(inventoryPotion).ToList();
            inventoryDictionary.Add(_item, newItem);
        }
    }
    public void RemoveItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
        {
            if (inventoryEquipmentDictionary.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackCount <= 1)
                {
                    inventoryEquipment.Remove(value);
                    inventoryEquipmentDictionary.Remove(_item);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item);
                }
                else
                    value.RemoveFromStack();
            }
        }
        else if (_item.itemType == ItemType.Material)
        {
            if (inventoryMaterialDictionary.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackCount <= 1)
                {
                    inventoryMaterial.Remove(value);
                    inventoryMaterialDictionary.Remove(_item);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item);
                }
                else
                    value.RemoveFromStack();
            }
        }
        else if (_item.itemType == ItemType.Potion)
        {
            if (inventoryPotionDictionary.TryGetValue(_item, out InventoryItem value))
            {
                if (value.stackCount <= 1)
                {
                    inventoryPotion.Remove(value);
                    inventoryPotionDictionary.Remove(_item);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item);
                }
                else
                    value.RemoveFromStack();
            }
        }
        UpdateSlotsUI();
    }

    public void RemoveItemByCount(InventoryItem _item)
    {
  
        if (_item.data.itemType == ItemType.Equipment)
        {
            if (inventoryEquipmentDictionary.TryGetValue(_item.data, out InventoryItem value))
            {
                value.stackCount -= _item.stackCount;
                if (value.stackCount <= 0)
                {
                    inventoryEquipment.Remove(value);
                    inventoryEquipmentDictionary.Remove(_item.data);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item.data);

                }
            }
        }
        else if (_item.data.itemType == ItemType.Material)
        {
            //RemoveFormAddBoth(_item.data);
            if (inventoryMaterialDictionary.TryGetValue(_item.data, out InventoryItem value))
            {
                value.stackCount -= _item.stackCount;
                if (value.stackCount <= 0)
                {
                    inventoryMaterial.Remove(value);
                    inventoryMaterialDictionary.Remove(_item.data);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item.data);
                }
            }
        }
        else if (_item.data.itemType == ItemType.Potion)
        {
            if (inventoryPotionDictionary.TryGetValue(_item.data, out InventoryItem value))
            {
                value.stackCount -= _item.stackCount;
                if (value.stackCount <= 0)
                {
                    inventoryPotion.Remove(value);
                    inventoryPotionDictionary.Remove(_item.data);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item.data);
                }
            }
        }
        UpdateSlotsUI();
    }

    public void RemoveItemByOne(InventoryItem _item)
    {
        if (_item.data.itemType == ItemType.Equipment)
        {
            if (inventoryEquipmentDictionary.TryGetValue(_item.data, out InventoryItem value))
            {
                value.stackCount--;
                if (value.stackCount <= 0)
                {
                    inventoryEquipment.Remove(value);
                    inventoryEquipmentDictionary.Remove(_item.data);
                    inventory.Remove(value);
                    inventoryDictionary.Remove(_item.data);
                }
            }
        }
        else if (_item.data.itemType == ItemType.Material)
        {
            if (inventoryMaterialDictionary.TryGetValue(_item.data, out InventoryItem value1))
            {
                value1.stackCount--;
                if (value1.stackCount <= 0)
                {
                    inventoryMaterial.Remove(value1);
                    inventoryMaterialDictionary.Remove(_item.data);
                    inventory.Remove(value1);
                    inventoryDictionary.Remove(_item.data);
                }
            }
        }
        else if (_item.data.itemType == ItemType.Potion)
        {
            if (inventoryPotionDictionary.TryGetValue(_item.data, out InventoryItem value2))
            {
                value2.stackCount--;
                if (value2.stackCount <= 0)
                {
                    inventoryPotion.Remove(value2);
                    inventoryPotionDictionary.Remove(_item.data);
                    inventory.Remove(value2);
                    inventoryDictionary.Remove(_item.data);
                    Debug.Log(_item.stackCount);
                }
            }
        }
        UpdateSlotsUI();
    }

    public bool CanCraft(ItemEquipmentData _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        //Ҫ�����ĵĲ���
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();
        //����Ƿ����㹻�Ĳ���
        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            //��ⵥ�������Ƿ����
            if(inventoryDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem value))
            {
                //��������������㣬�򷵻�false
                if(value.stackCount < _requiredMaterials[i].stackCount)
                {
                    return false;
                }
                else
                {
                    materialsToRemove.Add(_requiredMaterials[i]);
                }
            }
            //������ϲ����ڣ��򷵻�false
            else
            {
                return false;
            }
        }
        //���Ĳ���
        for (int i = 0; i < materialsToRemove.Count; i++) 
        {
            Debug.Log("Removing " + materialsToRemove[i].data.name + " " + materialsToRemove[i].stackCount);
            RemoveItemByCount(materialsToRemove[i]);
        }
        //�������Ʒ 
        AddItem(_itemToCraft);

        return true;
    }

    public ItemEquipmentData GetEquipment(EquipmentType _type)
    {
        ItemEquipmentData equipedItem = null;

        foreach (KeyValuePair<ItemEquipmentData, InventoryItem> item in equipmentDictionary)
        {
            if(item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }
}


