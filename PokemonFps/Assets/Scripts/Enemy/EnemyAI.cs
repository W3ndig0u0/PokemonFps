using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
  public NavMeshAgent agent;
  public Transform player;
  public LayerMask whatIsGround;

  void Awake()
  {
    player = GameObject.Find("Player").transform;
    agent = GetComponent<NavMeshAgent>();
  }

  void Update()
  {
    agent.SetDestination(player.position);
    // agent.LookAt(player.rotation);
  }

}