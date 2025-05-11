using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private bool canGrow = true;

    private float shrinkSpeed;
    private bool canShrink;

    private bool canCreateHotKeys = true;
    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    private bool canAttack;

    private bool playerCanDisapper = true;

    public List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        //��ʼ ��¡����
        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyHotKeys();

            if (targets.Count == 0)
            {
                EndBlackholeSkill();
                return;
            }

            canAttack = true;
            canCreateHotKeys = false;

            if (playerCanDisapper)
            {
                playerCanDisapper = false;
                PlayerManager.instance.player.fx.MakeTransparent(true);
            }
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            //�Ŵ�Ч��
            transform.localScale = Vector2.Lerp(transform.localScale,
                                                new Vector2(maxSize, maxSize),
                                                growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            //��СЧ��
            transform.localScale = Vector2.Lerp(transform.localScale,
                                                new Vector2(-1, -1),
                                                shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && canAttack && amountOfAttacks > 0) 
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;
            //����ж� ��¡ ��/�ҹ���
            if (Random.Range(0, 3) > 1)
                xOffset = 1;
            else
                xOffset = -1;
            //���� ��¡����
            SkillManager.instance.cloneSkill.CreateClone(targets[randomIndex], new Vector3(xOffset, 0, 0));

            amountOfAttacks--;
            //���������ﵽ���˳��ڶ�����״̬
            if (amountOfAttacks <= 0)
            {
                Invoke("EndBlackholeSkill", 1f);
            }

        }
    }

    private void EndBlackholeSkill()
    {
        PlayerManager.instance.player.ExitBlackholeAbility();
        canShrink = true;
        canAttack = false;
    }

    //�ڰ��� QTE���� �� ������ʧ
    private void DestroyHotKeys()
    {
        if (createdHotKey.Count < 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }
    //�Ӵ����ڶ��� �������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreatHotKey(collision);
        }
    }
    //���� ����״̬
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }
    //���� QTE����
    private void CreatHotKey(Collider2D collision)
    {
        if(keyCodeList.Count <= 0 )
        {
            Debug.Log("��λ����");
            return;
        }

        if (!canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab,
                        collision.transform.position + new Vector3(0, 2),
                        Quaternion.identity);
        createdHotKey.Add(newHotKey);

        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackholeHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotKeyController>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
