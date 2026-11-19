using UnityEngine;
using UnityEngine.InputSystem;

// Handles shooting by casting an invisible ray from the camera and checking what it hits.
public class Weapon : MonoBehaviour
{
    // The camera the raycast is fired from. Drag the Main Camera here, not the WeaponsCamera.
    [SerializeField] Camera FPCamera;
    // How far the raycast can reach, in Unity units (meters).
    [SerializeField] float range = 100f;
    // How long the gun must wait between shots (in seconds). It's a fixed setting that you adjust in the Inspector.
    [SerializeField] float shotCooldown = 0.5f;
    // The Input System action that listens for the fire button.
    InputAction shootAction;
    // A timer that adds shotCooldown when it starts.
    float nextShotTime = 0f;
    // Every layer except Weapons, so the gun's own colliders never block a shot.
    int shootMask;

    [SerializeField] private AmmoType ammoType;
    [SerializeField] private Ammo ammo;

    [SerializeField] private float damage = 1;

    [SerializeField] private ParticleSystem muzzelFlash;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] float hitEffectLifetime = 0.1f;

    // Runs once when the object loads.
    void Awake()
    {
        // The ~ flips the Weapons mask so it means "every layer except Weapons".
        shootMask = ~LayerMask.GetMask("Weapons");

        // Create the fire action and bind it to the left mouse button and the gamepad right trigger.
        shootAction = new InputAction("Shoot", InputActionType.Button);
        shootAction.AddBinding("<Mouse>/leftButton");
        shootAction.AddBinding("<Gamepad>/rightTrigger");

        // A prefab can't hold a reference to the Main Camera, so find it here if the slot is empty.
        if (FPCamera == null)
        {
            FPCamera = Camera.main;
        }
    }

    // Runs each time the object is turned on. An input action does nothing until it's enabled.
    void OnEnable()
    {
        shootAction.Enable();
    }

    // Runs each time the object is turned off, such as when the player switches weapons.
    void OnDisable()
    {
        shootAction.Disable();
    }

    // Runs when the object is removed. Dispose() releases the resources the input action holds.
    void OnDestroy()
    {
        shootAction.Dispose();
    }

    // Runs once per frame.
    void Update()
    {
        // WasPressedThisFrame() is true only on the frame the button goes down, so holding it won't repeat fire.
        if (shootAction.WasPressedThisFrame() && Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shotCooldown;
            Shoot();
        }
    }

    // Everything that happens when the gun fires.
    void Shoot()
    {
        ProcessRaycast();

        // Muzzle flash effect
        PlayMuzzleFlash();

        // TODO:
        // Integrate ammo system
        // Play a "shoot" sound?
    }

    // Fires the raycast and reports what it hits.
    void ProcessRaycast()
    {
        // A RaycastHit stores what the ray hit (transform), where (point), and which way the surface faces (normal).
        RaycastHit hit;

        // Four of the Raycast parameters: where from, which direction, what we hit, how far.
        // "out hit" fills in our RaycastHit. The mask skips the Weapons layer, and Ignore keeps trigger colliders such as pickups from blocking shots.
        if (Physics.Raycast(FPCamera.transform.position, FPCamera.transform.forward,
                out hit, range, shootMask, QueryTriggerInteraction.Ignore))
        {
            print("I hit this: " + hit.transform.name);
            // TODO: damage the target (added in the tutorial)
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();

            // Hit effect for visual feedback
            CreateImpact(hit);

            if (target == null)
            {
                return;
            }

            target.TakeDamage(damage);
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzelFlash.Play();
    }

    private void CreateImpact(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(impact, hitEffectLifetime);
    }
}
