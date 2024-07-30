using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    Unit unit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        unit = collision.GetComponent<Unit>();
    }
    public void enemyUnitAttack(float _enemyDamage)
    {
        unit.UnitHit(_enemyDamage);
    }

    public void enemyTowerAttack(float _enemyDamage)
    {
        towerStat.Instance.TowerHit(_enemyDamage);
    }

}
