using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHitBox : MonoBehaviour
{
    
    Unit unit;
    public enum playerUnitHitBoxType
    {
        attack,
        checkEnemy
    }

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    [SerializeField, Tooltip("�÷��̾� ������ ��Ʈ�ڽ� Ÿ��")] playerUnitHitBoxType hitBoxType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        unit.unitOnTriggerEnter(hitBoxType, collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        unit.unitOnTriggerExit(hitBoxType, collision);
    }
}
