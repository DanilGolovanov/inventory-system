using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Potion,
    Equipment,
    Default
}

public enum AttributeType
{
    Strength,
    Dexterity,
    Constitution,
    Wisdom,
    Intelligence,
    Charisma,
    Health,
    Mana,
    Stamina
}

public abstract class ItemObject : ScriptableObject
{
    public int id;
    public string name;
    public Sprite sprite;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public ItemBuff[] buffs;

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public ItemBuff[] buffs;

    public Item(ItemObject item)
    {
        name = item.name;
        id = item.id;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max);
            buffs[i].attribute = item.buffs[i].attribute;
        }
    }
}

[System.Serializable]
public class ItemBuff
{
    public AttributeType attribute;
    public int value;
    public int min;
    public int max;

    public ItemBuff(int min, int max)
    {
        this.min = min;
        this.max = max;
        GenerateValue();
    }

    public void GenerateValue()
    {
        value = Random.Range(min, max);
    }

}
