using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject damagePrefab;
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

    private void Start()
    {

    }

    public void createDamagePrint(int _damage, Vector2 _damagetrs, bool _isEnemy) 
    {
        GameObject go = Instantiate(damagePrefab, _damagetrs, Quaternion.identity);
        DamagePrint dmgPrint = go.GetComponent<DamagePrint>();
        dmgPrint.printDamage(_damage.ToString(), true, _isEnemy);
    }   
}
