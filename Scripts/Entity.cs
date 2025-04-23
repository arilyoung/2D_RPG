using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public BoxCollider2D bc { get; private set; }
    #endregion

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistace;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        anim = GetComponentInChildren<Animator>();

        rb = GetComponent<Rigidbody2D>();

        fx = GetComponent<EntityFX>();

        stats = GetComponent<CharacterStats>();

        bc = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {

    }

    //攻击命中 伤害函数
    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }

    //击倒函数
    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region SetVelocity
    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        //被击倒无法移动
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
                                                        Vector2.down,
                                                        groundCheckDistace,
                                                        whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position,
                                                    Vector2.right * facingDir,
                                                    wallCheckDistance,
                                                    whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        //地面检测线
        Gizmos.DrawLine(groundCheck.position,
                        new Vector3(groundCheck.position.x,
                                    groundCheck.position.y - groundCheckDistace));
        //墙面检测线
        Gizmos.DrawLine(wallCheck.position,
                        new Vector3(wallCheck.position.x + wallCheckDistance,
                                    wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    public virtual void Die()
    {
    }
}