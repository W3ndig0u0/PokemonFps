using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
//   public HealthBar healthbar;
  public float health;
  public float getScoreFromKill;
  public float maxHealth = 100;

  void Start()
  {
    // !Sätter max liv o Health bar
    health = maxHealth;
    // healthbar.SetMaxHealth(maxHealth);
  }
  public void TakeDamage(float damageAmount)
  {
    // !Skada
    health -= damageAmount;
    Debug.Log("Health: " + health);
    // !Minskar healthbar
    // healthbar.SetHealth(health);

    if (health <= 0f)
    {
      Die();
    }
  }
  void Die()
  {
    // !Dör
    gameObject.transform.rotation = Quaternion.Euler(-90, transform.rotation.x, transform.rotation.y);
    // Destroy(gameObject);
    // Score.scoreValue += getScoreFromKill;
  }

}