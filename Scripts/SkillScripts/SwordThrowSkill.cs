using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordThrowSkill : Skill
{
    public static SwordThrowSkill instance;

    public SwordType swordType = SwordType.Regular;

    [Header("Aim Arrow")]
    [SerializeField] private int numberOfArrows;
    [SerializeField] private float spaceBetweenArrows;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowsParent;

    [Header("ThrowSword info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;

    [Header("Bounce info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float peirceGravity;

    [Header("Spin info")]
    [SerializeField] private float spinGravity;
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float maxTravelDistance = 8;
    [SerializeField] private float hitCooldown;

    private Vector2 finalDir;

    private GameObject[] arrows;
    private float angleDegrees;
    private Vector2 arrowDir;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    protected override void Start()
    {
        bounceAmount -= 1;

        player = PlayerManager.instance.player;

        SetupGravity();

        GenerateArrows();
    }
    //设置不同状态下对剑的地心引力
    private void SetupGravity()
    {

        switch (swordType)
        {
            case SwordType.Regular:
                break;
            case SwordType.Bounce:
                swordGravity = bounceGravity;
                break;
            case SwordType.Pierce:
                swordGravity = peirceGravity;
                break;
            case SwordType.Spin:
                swordGravity = spinGravity;
                break;
        }
    }
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override bool UseSkillTrigger()
    {
        return base.UseSkillTrigger();
    }


    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {


            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x,
                                   AimDirection().normalized.y * launchForce.y);
            ArrowsActive(false);
        }
        //右键按住时显示抛物线指示
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].transform.position = ArrowsPosition(i * spaceBetweenArrows);
                arrows[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                if (i > 0)
                {
                    arrowDir = arrows[i].transform.position - arrows[i - 1].transform.position;
                    float angleRadians = Mathf.Atan2(arrowDir.y, arrowDir.x);
                    angleDegrees = angleRadians * Mathf.Rad2Deg;
                }
                arrows[i].transform.rotation = Quaternion.Euler(0, 0, angleDegrees - 90);
            }
        }
    }


    public override void UseSkill()
    {
        base.UseSkill();
    }


    //剑的创建
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        //扔剑状态切换
        switch (swordType)
        {
            case SwordType.Regular:
                break;
            case SwordType.Bounce:
                newSwordScript.SetupBounce(true, bounceAmount);
                break;
            case SwordType.Pierce:
                newSwordScript.SetupPierce(pierceAmount);
                break;
            case SwordType.Spin:
                newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, hitCooldown);
                break;
        }

        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssigneNewSword(newSword);
    }
    //瞄准抛物线功能实现
    #region AimRegion
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosiotn = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosiotn - playerPosition;

        return direction;
    }
    //是否显示抛物线
    public void ArrowsActive(bool _isActive)
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].SetActive(_isActive);
        }
    }
    //生成箭头
    private void GenerateArrows()
    {
        arrows = new GameObject[numberOfArrows];
        for (int i = 0; i < numberOfArrows; i++)
        {
            arrows[i] = Instantiate(arrowPrefab, player.transform.position, Quaternion.identity, arrowsParent);
            arrows[i].SetActive(false);
        }
    }
    //箭头位置设置
    private Vector2 ArrowsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
