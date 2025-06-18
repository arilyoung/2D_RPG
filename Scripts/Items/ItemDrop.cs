using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleBasicDrop;
    [SerializeField] private ItemData[] possibleRareDrop;
    [SerializeField] private ItemData[] possibleEpicDrop;
    private List<ItemData> dropList;

    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private ItemData item;

    private EnemyStats enemyStats;

    [Header("Enemy Drops")]
    public float baseicDropChance = .6f;
    public float rareDropChance = .2f;
    public float epicDropChance = .02f;

    private float dropBasicChance;
    private float dropRareChance;
    private float dropEpicChance;

    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        dropList = new List<ItemData>();
    }

    public void GenerateDrop()
    {
        //基础物品掉率计算 60% - (敌人等级-1)*10% - (玩家幸运值)*10%
        dropBasicChance = baseicDropChance -
                ((enemyStats.level - 1) * 0.1f) -
                (((PlayerManager.instance.player.GetComponent<PlayerStats>().Lucky.GetValue()) * 0.1f));

        //稀有物品掉率计算 20% + (敌人等级-1)*10% + (玩家幸运值)*10%
        dropRareChance = rareDropChance +
                ((enemyStats.level - 1) * 0.1f) +
                (((PlayerManager.instance.player.GetComponent<PlayerStats>().Lucky.GetValue()) * 0.1f));
        //史诗级物品掉率 (敌人等级>=5)
        //（ 2% + (玩家幸运值)*2% ）* （1 + (敌人等级-1)*10%）
        if (enemyStats.level >= 5)
        {
            dropEpicChance = (epicDropChance +
                ((PlayerManager.instance.player.GetComponent<PlayerStats>().Lucky.GetValue()) * 0.02f)) *
                (1 + (enemyStats.level - 1) * 0.1f);
        }
        else
        {
            dropEpicChance = 0;
        }

        //生成掉落物品列表(最多3轮判断)
        for (int i = 0; i < 3; i++)
        {
            if (dropList.Count >= 5)
            {
                break;
            }
            if (Random.Range(0.0f, 1.0f) <= dropBasicChance)
            {
                dropList.Add(possibleBasicDrop[Random.Range(0, possibleBasicDrop.Length)]);
            }
            if (Random.Range(0.0f, 1.0f) <= dropRareChance)
            {
                dropList.Add(possibleRareDrop[Random.Range(0, possibleRareDrop.Length)]);
            }
            if (Random.Range(0.0f, 1.0f) <= dropEpicChance)
            {
                dropList.Add(possibleEpicDrop[Random.Range(0, possibleEpicDrop.Length)]);
            }
        }

        //生成掉落物品
        for (int i = 0; i < dropList.Count; i++)
        {
            DropItem(dropList[i]);
        }

        dropList.RemoveAll(item => item == null);
    }


    //生成掉落物品

    public void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
