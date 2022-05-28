using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
  public NavMeshAgent agent;
  public Transform player;
  public LayerMask whatIsGround;
  Target target;
  
  void Awake()
  {
    player = GameObject.Find("Player").transform;
    agent = GetComponent<NavMeshAgent>();
    target = GetComponent<Target>();
  }

  void Update()
  {
    if (!target.dead)
    {
      agent.SetDestination(player.position);
    }

  }

}