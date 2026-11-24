using UnityEngine;

// Tracks an enemy's health and whether it's dead.
public class EnemyHealth : MonoBehaviour
{
    // How much damage the enemy can take before it dies.
    [SerializeField] float hitPoints = 100f;

    // True once the enemy has died. Other scripts check it through IsDead().
    public bool isDead = false;
    // The enemy's collider, which gets turned off when the enemy dies.
    Collider enemyColl;

    // Runs once when the object loads.
    void Awake()
    {
        // Grab the collider on this same GameObject.
        enemyColl = GetComponent<Collider>();
    }

    // Lets other scripts ask whether the enemy is dead.
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        BroadcastMessage("OnDamageTaken");

        hitPoints -= damage;

        if (hitPoints <= damage)
        {
            //Destroy(gameObject);
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        enemyColl.enabled = false;

        GetComponent<Animator>().SetTrigger("die");
    }
    // Students add Die() in Week 16.
}
