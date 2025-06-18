using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeSkill : Skill
{
    public static BlackholeSkill instance;

    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCooldown;
    [Space]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    public override bool UseSkillTrigger()
    {
        return base.UseSkillTrigger();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        //´´½¨ºÚ¶´
        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        BlackholeSkillController newBlackholeScript = newBlackhole.GetComponent<BlackholeSkillController>();

        newBlackholeScript.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCooldown);
    }

    protected override void Start()
    {
        base.Start();
        player = PlayerManager.instance.player;
    }

    protected override void Update()
    {
        base.Update();
    }
}
