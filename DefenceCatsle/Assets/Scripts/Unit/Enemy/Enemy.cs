using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool isAttack = false;
    void Awake()
    {
        isAttack = false;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.CompareTag("Enemy") && collision.tag == "Player")
        { 
            isAttack = true;
        }
    }


    void Update()
    {
        EnemyMove(3);
    }

    private void EnemyMove(float _speed) 
    {
        if(isAttack == false) 
        {
            gameObject.transform.position +=  Vector3.left * _speed * Time.deltaTime;
        }
        if (isAttack == true) 
        {
           
        }
    }
}
