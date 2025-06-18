using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dashSkill { get; private set; }
    public CloneSkill cloneSkill { get; private set; } 
    public SwordThrowSkill swordThrowSkill { get; private set; }
    public BlackholeSkill blakcholeSkill { get; private set; }
    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordThrowSkill = GetComponent<SwordThrowSkill>();
        blakcholeSkill = GetComponent<BlackholeSkill>();
    }
}
