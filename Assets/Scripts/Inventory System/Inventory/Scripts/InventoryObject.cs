using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    [SerializeField]
    private string savePath = "/inventory.sav";
    public ItemDatabaseObject database;
    public Inventory Container;

    public void AddItem(Item item, int amount)
    {
        if (item.buffs.Length > 0)
        {
            SetFirstEmptySlot(item, amount);
            return;
        }

        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.id == item.id)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        }
        SetFirstEmptySlot(item, amount);
    }

    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].id <= -1)
            {
                Container.Items[i].UpdateSlot(item.id, item, amount);
                return Container.Items[i];
            }
        }
        // setup functionality for when you inventory is full
        return null;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        //string saveData = JsonUtility.ToJson(this, true);
        //BinaryFormatter formatter = new BinaryFormatter();
        //FileStream stream = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //formatter.Serialize(stream, saveData);
        //stream.Close();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.dataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.dataPath, savePath)))
        {
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream stream = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            //JsonUtility.FromJsonOverwrite(formatter.Deserialize(stream).ToString(), this);
            //stream.Close();

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.dataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[42];
}

[System.Serializable]
public class InventorySlot
{
    public int id = -1;
    public Item item;
    public int amount;

    public InventorySlot()
    {
        this.id = -1;
        this.item = null;
        this.amount = 0;
    }

    public InventorySlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public void UpdateSlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
