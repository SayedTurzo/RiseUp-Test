using UnityEngine;

public abstract class MagnetBase : MonoBehaviour
{
    public bool isNorthPole;           // Whether this magnet is a North Pole (true) or South Pole (false)
    public float magnetRange = 5f;     // Range in which the magnet can interact with other objects
    public LayerMask affectedLayers;   // Layer mask to define which objects can be affected by the magnet

    // You can add some common behavior for both static and dynamic magnets here
    protected virtual void Start()
    {
        // Common setup code can be placed here
    }

    // Abstract method that subclasses must implement for specific magnetic interactions
    protected abstract void CheckForMagneticInteraction();

    // You can also provide a common method to visualize the range in the editor
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = isNorthPole ? Color.red : Color.blue;
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}