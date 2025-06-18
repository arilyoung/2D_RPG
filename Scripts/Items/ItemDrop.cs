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
        //������Ʒ���ʼ��� 60% - (���˵ȼ�-1)*10% - (�������ֵ)*10%
        dropBasicChance = baseicDropChance -
                ((enemyStats.level - 1) * 0.1f) -
                (((PlayerManager.instance.player.GetComponent<PlayerStats>().Lucky.GetValue()) * 0.1f));

        //ϡ����Ʒ���ʼ��� 20% + (���˵ȼ�-1)*10% + (�������ֵ)*10%
        dropRareChance = rareDropChance +
                ((enemyStats.level - 1) * 0.1f) +
                (((PlayerManager.instance.player.GetComponent<PlayerStats>().Lucky.GetValue()) * 0.1f));
        //ʷʫ����Ʒ���� (���˵ȼ�>=5)
        //�� 2% + (�������ֵ)*2% ��* ��1 + (���˵ȼ�-1)*10%��
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

        //���ɵ�����Ʒ�б�(���3���ж�)
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

        //���ɵ�����Ʒ
        for (int i = 0; i < dropList.Count; i++)
        {
            DropItem(dropList[i]);
        }

        dropList.RemoveAll(item => item == null);
    }


    //���ɵ�����Ʒ

    public void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
