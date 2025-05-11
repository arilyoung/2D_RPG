using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //��֤һ����ֻ��һ��ʵ�� �������÷��ʸ�ʵ����ȫ�ַ��ʵ� 
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        //��ֹ��������
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
 