using UnityEngine;

public class MeleeEnemy : Enemy
{
    
    void Start()
    {
        
    }

    void Update()
    {
        Patrol();
        FindPlayer();

        _maxHealth = 5;
    }

    void ChasePlayer()
    {
        base.Attack();//Llama a la funcion virtual
    }
}
