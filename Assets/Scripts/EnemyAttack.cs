using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth target;

    [SerializeField] float damage = 40f;

    private void Start()
    {
        target = FindAnyObjectByType<PlayerHealth>();
    }

    public void AttackHitEvent()
    {
        if (target == null)
        {
            return;
        }

        //print("Ouch!");
        target.PlayerTakeDamage(damage);
    }
}
