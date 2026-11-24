using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // How much damage the player can take before it dies.
    [SerializeField] float hitPoints = 100f;
    public void PlayerTakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= damage)
        {
            print("Player died!");
        }
    }
    // Students add Die() in Week 16.
}
