using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/LootFortune")]
public class LootFortune : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;

    public LootFortune(string lootName, int dropChance)
    {
        this.lootName = lootName;
        this.dropChance = dropChance;
    }
}
