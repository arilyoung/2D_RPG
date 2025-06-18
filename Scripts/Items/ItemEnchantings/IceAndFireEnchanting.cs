using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and Fire Enchanting", menuName = "Data/Item Enchanting/Ice and Fire Enchanting")]

public class IceAndFireEnchanting : ItemEnchanting
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void ExecuteEnchanting(Transform _respawnPosition)
    {
        Player player = PlayerManager.instance.player;

        bool isThirdAttack = player.GetComponent<Player>().primaryAttackState.comboCounter == 2;

        //if (isThirdAttack)
        //{
        //    GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.rotation);
        //    newIceAndFire.GetComponent<Rigidbody2D>().velocity = velocity;
        //}

        GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);

        newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);

        Destroy(newIceAndFire, 10f);
    }
}
