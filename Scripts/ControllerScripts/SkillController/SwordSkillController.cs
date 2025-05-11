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

    //���ý���������
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
    //���õ�������
    public void SetupBounce(bool _isBouncing,int _bounceAmount)
    {
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;

        enemyTarget = new List<Transform>();
    }
    //���ô̴�����
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    //������ת����
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance,float _spinDuration,float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    //��������������
    public void ReturnSword()
    {
        //ֹͣ�ƶ�
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
            //ֱ�ӳ���ɫ�����ƶ�
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            //���������ɫ��ߵõ������٣���
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }
    //��ת�߼�
    private void SpinLogic()
    {
        if (isSpinning)
        {
            //����ָ����ԶĿ����Զ�ֹͣ�ƶ�
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                //��ײ����һ�����˺�����ƶ�
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x + spinDiraction, transform.position.y),
                1f * Time.deltaTime);
                hitTimer -= Time.deltaTime;
                //��תʱ�������
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
                //ʱ�䵽�Զ�����
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;

                }
            }
        }
    }
    //���н�·;��ֹͣ��������һ�����ˣ�
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    //�����߼�
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0 && isReturning == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, 20 * Time.deltaTime);
            //�����ӽ�����ʱ�����д����ת��Ŀ��
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
        //������ʱ��������ײ���
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
    //�ü���״̬�¹����з���ɽ�ֱ
    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.DamageImpact();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    //�������˼��
    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                //���˼�ⷶΧ
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }
    //�����������ߵ���
    private void StuckInto(Collider2D collision)
    {
        //����״̬���ж��Ƿ�������
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
        //�Ӵ��������ײ
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
