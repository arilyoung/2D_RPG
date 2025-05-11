using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //保证一个类只有一个实例 并且设置访问该实例的全局访问点 
    public static PlayerManager instance;
    public Player player;

    private void Awake()
    {
        //防止多对象存入
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
}
 