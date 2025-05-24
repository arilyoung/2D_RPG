using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; //���� ��ǿ�˺� �ٷֱ�����
    public Stat agility; //���� ��ǿ������ ������ �ٷֱ�����
    public Stat intelligence; //���� ��ǿħ���˺� ħ���ֿ� �ٷֱ�����
    public Stat vitality; //���� ��ǿ����ֵ ��ֵ����

    [Header("Offensive stats")]
    public Stat damage; //�����˺�
    public Stat critChance; //������
    public Stat critPower; //��������

    [Header("Defensive stats")]
    public Stat maxHealth; //�������ֵ
    public Stat evasion; //����
    public Stat armor; //����ֵ
    public Stat magicResistence; //ħ��ֵ

    [Header("Magic stats")]
    public Stat fireDamage; //��ϵħ���˺�
    public Stat iceDamage; //��ϵħ���˺�
    public Stat lightingDamage; //��ϵħ���˺�

    public bool isIgnited; //��ϵ״̬ ��������˺�
    public bool isChilled; //��ϵ״̬ ������� ��������ֵ 30%��δʵ�֣�
    public bool isShocked; //��ϵ״̬ ʹ���˸е� ��ɴ����˺�

    private float ignitedTimer; //ȼ�ճ���ʱ��
    private float ignitedDamageCooldown = .2f; //ȼ���˺���ɼ��
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
        //��״̬��ʱ
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        //��Ԫ�ظ������˺������ʱ
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;

        if (chilledTimer < 0)
            isChilled = false;

        if (shockedTimer < 0)
            isShocked = false;

        //��ɻ�Ԫ�ظ����˺�
        if (isIgnited)
            ApplyIgniteDamage();
    }

    #region ħ���Լ�����Ч��
    //����ħ���˺����� ������ʩ��Ԫ�ظ��溯����
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
    //����ħ���˺� ħ��ֵ������˺� ����
    private static float CheckTargetMagicResistance(CharacterStats _targetStats, float totalMagicDamage)
    {
        //ħ������
        totalMagicDamage *= (1 - ((_targetStats.magicResistence.GetValue() + (_targetStats.intelligence.GetValue() *  .5f) *  .01f)));
        //�޷���ȫ����ħ���˺�
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 1, int.MaxValue);
        totalMagicDamage = Mathf.RoundToInt(totalMagicDamage);
        return totalMagicDamage;
    }
    //Ԫ�ظ���Ч��
    public void ApplyElementDebuff(bool _ignited, bool _chill, bool _shock)
    {
        //Ԫ�ظ���Ч�����������
        bool canApplyIgnite = !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;

        if(_ignited && canApplyIgnite)
        {
            isIgnited = _ignited;
            ignitedTimer = 4;
            //��Ԫ���Ӿ�Ч��
            fx.IgniteFxFor(ignitedTimer);
        }
        if(_chill && canApplyChill)
        {
            isChilled = _chill;
            chilledTimer = 3;
            //��Ԫ���Ӿ�Ч��
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
    //��Ԫ�س�������
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
    //��Ԫ�ش���Ч��
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = 4;
        //��Ԫ���Ӿ�Ч��
        fx.ShockFxFor(shockedTimer);
    }
    //�����������
    private void HitNearestEnemyWithStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 15);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            //���������ʱ ��ֹ���������Լ�
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
        //ֻ��һ�����˵�ʱ��Ҳ����
        if (closestEnemy == null)
            closestEnemy = transform;
        //���ɵ��Ԥ����������ĵ���
        if (closestEnemy != null)
        {
            GameObject newShockStirke = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStirke.GetComponent<ShockStrike_Controller>().Setup((int)shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    #endregion

    #region ����Ԫ���˺�Ӧ��
    //���û�Ԫ�ظ���Ч���˺�
    public void SetupIgniteDamage(int _damage) => ignitedDamage = _damage;
    //������Ԫ�ظ���Ч���˺�
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
    #endregion

    #region �����˺�����
    //���������˺�����
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //�ж�����
        if (TargetCanAvoidAttack(_targetStats))
            return;

        //������������˺�
        int totalDamage = damage.GetValue() + strength.GetValue();

        //�жϱ��� ���Ҽ��㱩���˺�
        if (CanCrit())
            totalDamage = CalculateCriticalDamage(totalDamage);

        //���㻤���������˺�
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        //����˺�
        _targetStats.TakeDamage(totalDamage);
    }
    //���㷴���˺�����(���������˺�)
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
    //�ܷ񱩻��ж�
    private bool CanCrit()
    {
        int totalCritcalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 101) <= totalCritcalChance)
            return true;
        return false;
    }
    //�����˺�����
    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = critPower.GetValue() * .01f;

        float critDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }
    //���������˺� ����ֵ������˺� ����
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
    //�����˺�����
    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();

        if (currentHealth <= 0 && !isDead)
            Die();
    }
    //���ܹ�������
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 30;

        if (Random.Range(0, 100) < totalEvasion) // �޷�ʵ�� 100% ����
        {
            return true;
        }
        return false;
    }
    #endregion

    //���Ѫ�����޺���
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
    //Ѫ���½�����
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;
        if (onHealthChanged != null)
            onHealthChanged();
    }
    //ʵ����������
    protected virtual void Die()
    {
        isDead = true;
    }
}
