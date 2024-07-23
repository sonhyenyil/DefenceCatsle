using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    public enum enemyHitBoxType
    {
        checkPlayer,
        attack
    }

    Enemy enemy;
    [SerializeField] enemyHitBoxType enemyHit;
    EnemyAttack enemyAtkCol;

    private void Awake()
    {
        enemyAtkCol = GetComponentInParent<EnemyAttack>();
    }

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.enemyOnTirrgerEnter(enemyHit, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.enemyOnTirrgerExit(enemyHit, collision);
    }
}
