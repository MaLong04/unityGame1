using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemie : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;

    public Animator AnimatorController;
    public NavMeshAgent Agent;

    private float lastAttackTime = 0;
    private bool isDead = false;

    private void Start()
    {
        GameSceneManager.Instance.AddEnemie(this);
        Agent.SetDestination(GameSceneManager.Instance.Player.transform.position);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (Hp <= 0)
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        var distance = Vector3.Distance(transform.position, GameSceneManager.Instance.Player.transform.position);

        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                GameSceneManager.Instance.Player.Hp -= Damage;
                AnimatorController.SetTrigger("Attack");
            }
        }
        else
        {
            Agent.SetDestination(GameSceneManager.Instance.Player.transform.position);
        }
        AnimatorController.SetFloat("Speed", Agent.velocity.magnitude);
    }

    protected virtual void Die()
{
        GameSceneManager.Instance.RemoveEnemie(this); // Удаление врага
    isDead = true;
    AnimatorController.SetTrigger("Die");

        // Восстанавливаем HP игроку
        GameSceneManager.Instance.Player.RestoreHealth(10f); // Например, восстанавливаем 10 HP
    
}


}
