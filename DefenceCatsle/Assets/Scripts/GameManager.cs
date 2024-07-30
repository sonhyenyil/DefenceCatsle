using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject damagePrefab;
    [SerializeField] Transform damageCreateParentObj;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void createDamagePrint(int _damage, Vector2 _damagetrs, bool _isEnemy) 
    {
        GameObject go = Instantiate(damagePrefab, _damagetrs, Quaternion.identity, damageCreateParentObj);
        DamagePrint dmgPrint = go.GetComponent<DamagePrint>();
        dmgPrint.printDamage(_damage.ToString(), true, _isEnemy);
    }   
}
