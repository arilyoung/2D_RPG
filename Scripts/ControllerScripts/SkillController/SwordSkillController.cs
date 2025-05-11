using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private float freezeTimeDuration;

    private float returnSpeed;
    private bool isReturning;

    private bool canRotate = true;

    private bool isBouncing;
    private int bounceAmount;

    private bool isPiercing;
    private int pierceAmount;

    private bool isSpinning;
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private float spinDiraction;

    private float hitTimer;
    private float hitCooldown;

    private List<Transform> enemyTarget;
    private int targetIndex;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroySword()
    {
        Destroy(gameObject);
    }

    //设置剑基础属性
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);

        spinDiraction = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroySword", 8);
    }
    //设置弹跳属性
    public void SetupBounce(bool _isBouncing,int _bounceAmount)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;

        enemyTarget = new List<Transform>();
    }
    //设置刺穿属性
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    //设置旋转属性
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance,float _spinDuration,float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    //剑返回属性设置
    public void ReturnSword()
    {
        //停止移动
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            //直接朝角色方向移动
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            //当剑到达角色身边得到（销毁）剑
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }
    //旋转逻辑
    private void SpinLogic()
    {
        if (isSpinning)
        {
            //到达指定最远目标后自动停止移动
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                //在撞到第一个敌人后继续移动
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x + spinDiraction, transform.position.y),
                1f * Time.deltaTime);
                hitTimer -= Time.deltaTime;
                //旋转时打击敌人
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
                //时间到自动返回
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;

                }
            }
        }
    }
    //在行进路途中停止（遇见第一个敌人）
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    //弹跳逻辑
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0 && isReturning == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, 20 * Time.deltaTime);
            //当剑接近敌人时，进行打击并转换目标
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) <  .1f )
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //剑返回时不进行碰撞检测
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }
        //collision.GetComponent<Enemy>()?.Damage();

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }
    //该技能状态下攻击敌方造成僵直
    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.DamageImpact();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    //弹跳敌人检测
    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //敌人检测范围
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }
    //剑插入地面或者敌人
    private void StuckInto(Collider2D collision)
    {
        //穿刺状态下判断是否插入敌人
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        //接触后结束碰撞
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
