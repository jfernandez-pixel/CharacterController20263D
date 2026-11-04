using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _maxHealth;
    public float _attackDamage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void FindPlayer()
    {

    }

    public void Patrol()
    {
        
    }

    public virtual void Attack()// Virtual permite sobreescribir la funcion en otro script
    {

    }

    public void TakeDamage()
    {
        Debug.Log("Recibiendo daño");
    }
}
