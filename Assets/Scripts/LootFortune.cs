using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Inventory.Model
{
    [CreateAssetMenu(menuName = "ScriptableObject/LootFortune")]
    public class LootFortune : ScriptableObject
    {
        [field: SerializeField] public Sprite lootSprite { get; set; }
        [field: SerializeField] public string lootName { get; set; }
        [field: SerializeField] public int dropChance;
        [field: SerializeField] public bool IsStackable { get; set; }
        public int ID => GetInstanceID();
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;


        public LootFortune(string lootName, int dropChance)
        {
            this.lootName = lootName;
            this.dropChance = dropChance;
        }
    }
}

