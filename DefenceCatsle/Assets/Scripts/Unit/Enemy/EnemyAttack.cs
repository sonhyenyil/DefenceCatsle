using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public void enemyUnitAttack(float _enemyDamage)
    {

    }

    public void enemyTowerAttack(float _enemyDamage)
    {
        towerStat.Instance.TowerHit(_enemyDamage);
    }

}
