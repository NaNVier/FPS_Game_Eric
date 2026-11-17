using UnityEngine;

// Keeps track of how much ammo the player has for each type. It goes on the Player.
public class Ammo : MonoBehaviour
{
    // One slot per ammo type. Add and fill them in the Inspector.
    [SerializeField] AmmoSlot[] ammoSlots;

    // A pairing of one ammo type with how many rounds the player has of it.
    // [System.Serializable] is what lets the Inspector show it.
    [System.Serializable]
    private class AmmoSlot
    {
        // Picked from the drop-down made by the AmmoType enum.
        public AmmoType ammoType;
        public int ammoAmount;
    }

    // Returns how many rounds the player has of the given type.
    public int GetCurrentAmmo(AmmoType ammoType)
    {
        return GetAmmoSlot(ammoType).ammoAmount;
    }

    // Uses up one round of the given type. Mathf.Max keeps it from going below zero.
    public void ReduceCurrentAmmo(AmmoType ammoType)
    {
        AmmoSlot slot = GetAmmoSlot(ammoType);
        slot.ammoAmount = Mathf.Max(0, slot.ammoAmount - 1);
    }

    // Adds rounds of the given type, such as when the player picks up ammo.
    public void IncreaseCurrentAmmo(AmmoType ammoType, int ammoAmount)
    {
        GetAmmoSlot(ammoType).ammoAmount += ammoAmount;
    }

    // Finds the slot that matches the given ammo type. Returns null if there isn't one.
    private AmmoSlot GetAmmoSlot(AmmoType ammoType)
    {
        foreach (AmmoSlot slot in ammoSlots)
        {
            if (slot.ammoType == ammoType)
            {
                return slot;
            }
        }
        return null;
    }
}
