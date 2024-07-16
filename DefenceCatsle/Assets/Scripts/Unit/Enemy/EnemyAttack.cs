using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] float enemyDamage = 200.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyAttack(enemyDamage);
        }
    }

    private void enemyAttack(float _enemyDamge)
    {

        towerStat.Instance.TowerHit(_enemyDamge);
    }

}
