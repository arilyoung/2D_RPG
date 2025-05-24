using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; //力量 增强伤害 百分比增加
    public Stat agility; //敏捷 增强闪避率 暴击率 百分比增加
    public Stat intelligence; //智力 增强魔法伤害 魔法抵抗 百分比增加
    public Stat vitality; //体力 增强生命值 数值增加

    [Header("Offensive stats")]
    public Stat damage; //基础伤害
    public Stat critChance; //暴击率
    public Stat critPower; //暴击倍率

    [Header("Defensive stats")]
    public Stat maxHealth; //最大生命值
    public Stat evasion; //闪避
    public Stat armor; //护甲值
    public Stat magicResistence; //魔抗值

    [Header("Magic stats")]
    public Stat fireDamage; //火系魔法伤害
    public Stat iceDamage; //冰系魔法伤害
    public Stat lightingDamage; //雷系魔法伤害

    public bool isIgnited; //火系状态 持续造成伤害
    public bool isChilled; //冰系状态 冻结敌人 削减护甲值 30%（未实现）
    public bool isShocked; //雷系状态 使敌人感电 造成传导伤害

    private float ignitedTimer; //燃烧持续时间
    private float ignitedDamageCooldown = .2f; //燃烧伤害造成间隔
    private float ignitedDamageTimer;
    private float ignitedDamage;

    [SerializeField] private GameObject shockStrikePrefab;
    private float shockDamage;

    private float chilledTimer;
    private float shockedTimer;

    public int currentHealth;

    public System.Action onHealthChanged;

    protected bool isDead;

    protected virtual void Start()
    {
        critChance.SetDefultValue(5);
        critPower.SetDefultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    void Update()
    {
        //各状态计时
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        //火元素负面多段伤害间隔计时
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        //造成火元素负面伤害
        if (isIgnited)
            ApplyIgniteDamage();
    }

    #region 魔法以及负面效果
    //计算魔法伤害函数 （并且施加元素负面函数）
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        float totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicDamage = CheckTargetMagicResistance(_targetStats, totalMagicDamage);

        _targetStats.TakeDamage((int)totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            int value = Random.Range(0, 3);
            if(value == 0 && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyElementDebuff(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (value == 1 && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyElementDebuff(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (value == 2 && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyElementDebuff(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .5f));

        _targetStats.ApplyElementDebuff(canApplyIgnite, canApplyChill, canApplyShock);

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    //承受魔法伤害 魔抗值计算后伤害 函数
    private static float CheckTargetMagicResistance(CharacterStats _targetStats, float totalMagicDamage)
    {
        //魔抗乘算
        totalMagicDamage *= (1 - ((_targetStats.magicResistence.GetValue() + (_targetStats.intelligence.GetValue() *  .5f) *  .01f)));
        //无法完全免疫魔法伤害
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 1, int.MaxValue);
        totalMagicDamage = Mathf.RoundToInt(totalMagicDamage);
        return totalMagicDamage;
    }
    //元素负面效果
    public void ApplyElementDebuff(bool _ignited, bool _chill, bool _shock)
    {
        //元素负面效果不异类叠加
        bool canApplyIgnite = !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if(_ignited && canApplyIgnite)
        {
            isIgnited = _ignited;
            ignitedTimer = 4;
            //火元素视觉效果
            fx.IgniteFxFor(ignitedTimer);
        }
        if(_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = 3;
            //冰元素视觉效果
            fx.ChillFxFor(chilledTimer);
        }
        if(_shock && canApplyShock)
        {
            if(!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                HitNearestEnemyWithStrike();
            }
        }
    }
    //火元素持续灼烧
    private void ApplyIgniteDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy((int)ignitedDamage);
            ignitedDamageTimer = ignitedDamageCooldown;

            if (currentHealth <= 0 && !isDead)
                Die();
        }
    }
    //雷元素传递效果
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = 4;
        //电元素视觉效果
        fx.ShockFxFor(shockedTimer);
    }
    //攻击最近敌人
    private void HitNearestEnemyWithStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            //当命中玩家时 防止射出闪电打自己
            if (hit.GetComponent<Player>() != null && hit.transform == transform)
                return;
            
            if (hit.GetComponent<Enemy>() != null && hit.transform != transform)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        //只有一个敌人的时候也会打击
        if (closestEnemy == null)
            closestEnemy = transform;
        //生成电击预制体打击最近的敌人
        if (closestEnemy != null)
        {
            GameObject newShockStirke = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStirke.GetComponent<ShockStrike_Controller>().Setup((int)shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion

    #region 负面元素伤害应用
    //设置火元素负面效果伤害
    public void SetupIgniteDamage(int _damage) => ignitedDamage = _damage;
    //设置雷元素负面效果伤害
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    #region 物理伤害计算
    //计算物理伤害函数
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //判断闪避
        if (TargetCanAvoidAttack(_targetStats))
            return;

        //计算基础物理伤害
        int totalDamage = damage.GetValue() + strength.GetValue();

        //判断暴击 并且计算暴击伤害
        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);

        //计算护甲削弱后伤害
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        //造成伤害
        _targetStats.TakeDamage(totalDamage);
    }
    //计算反击伤害函数(三倍物理伤害)
    public virtual void DoCounterAttackDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = (damage.GetValue() + strength.GetValue()) * 3;

        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }
    //能否暴击判断
    private bool CanCrit()
    {
        int totalCritcalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 101) <= totalCritcalChance)
            return true;
        return false;
    }
    //暴击伤害计算
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = critPower.GetValue() * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    //承受物理伤害 护甲值计算后伤害 函数
    private static int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .7f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        if (totalDamage <= 0)
            totalDamage = 1;

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    //承受伤害函数
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    //闪避攻击函数
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 30;

        if (Random.Range(0, 100) < totalEvasion) // 无法实现 100% 闪避
        {
            return true;
        }
        return false;
    }
    #endregion

    //最大血量上限函数
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    //血量下降函数
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }
    //实体死亡函数
    protected virtual void Die()
    {
        isDead = true;
    }
}
