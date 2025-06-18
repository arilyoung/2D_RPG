using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Thunder Strike Enchanting", menuName = "Data/Item Enchanting/Thunder Strike Enchanting")]
public class ThunderStrikeEnchanting : ItemEnchanting
{
    //�׻�Ԥ����
    [SerializeField] private GameObject thunderStrikePrefab;
    //ִ�и�ħЧ��
    public override void ExecuteEnchanting(Transform _enemyPosition)
    {
        //ʵ�����׻�Ԥ����
        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
   