using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private SpriteRenderer sr;

    private Transform myEnemy;
    private BlackholeSkillController blackHole;
    public void SetupHotKey(KeyCode _myNewHotKey, Transform _myEnemy, BlackholeSkillController _myBlackhole)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();

        myEnemy = _myEnemy;
        blackHole = _myBlackhole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}