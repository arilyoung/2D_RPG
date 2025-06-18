using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Thunder Strike Enchanting", menuName = "Data/Item Enchanting/Thunder Strike Enchanting")]
public class ThunderStrikeEnchanting : ItemEnchanting
{
    //雷击预制体
    [SerializeField] private GameObject thunderStrikePrefab;
    //执行附魔效果
    public override void ExecuteEnchanting(Transform _enemyPosition)
    {
        //实例化雷击预制体
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
   