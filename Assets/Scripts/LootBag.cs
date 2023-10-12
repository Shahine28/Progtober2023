using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] List<LootFortune> lootList = new List<LootFortune>();
    public 
    // Start is called before the first frame update
    LootFortune GetDroppedItem()
    {
        int RandomNumber = Random.Range(1, 101);//1-100
        List<LootFortune> possibleItems = new List<LootFortune>();
        foreach(LootFortune item in lootList)
        {
            if (RandomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if (possibleItems.Count > 0)
        {
            LootFortune droppedItem = possibleItems[Random.Range(0, possibleItems.Count)];// je prends un item au hasard parmi tout ceux s�l�ctionn�.
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    public void InstantiateLoot()
    {
        /*List<LootFortune> droppedItem = GetDroppedItem();*/ /*Dans le cas o� je dropp plusieurs items.*/
        LootFortune droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {
            GameObject lootGameObject = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
            lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
        }
    }

/*    List<LootFortune> GetDroppedItem()
    {
        int RandomNumber = Random.Range(1, 101);//1-100
        List<LootFortune> possibleItems = new List<LootFortune>();
        foreach (LootFortune item in lootList)
        {
            if (RandomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
                return possibleItems;
            }
        }
    } Dans le cas o� je voudrais que plusieur itemps soient lach�s*/
}
