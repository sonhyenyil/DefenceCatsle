using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected float curUnitHp = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected void UnitHit(float _damage)
    {
        if (curUnitHp > 0)
        {
            curUnitHp -= _damage;
        }
        else if (curUnitHp <= 0)
        {
           curUnitHp = 0;
           Destroy(gameObject);
        }
    }

}
