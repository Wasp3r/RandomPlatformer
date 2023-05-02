using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     This class is a base for all picking controllers.
    /// </summary>
    public abstract class PickingController : MonoBehaviour
    {
        /// <summary>
        ///     The layer mask of the objects that the player can pick up.
        /// </summary>
        [SerializeField] private LayerMask _colliderMask;

        /// <summary>
        ///     Action to be done when the player picks up an object.
        /// </summary>
        protected abstract void OnPickedUp(GameObject other);
        
        /// <summary>
        ///     The collider to pick up coins.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_colliderMask != (_colliderMask | (1 << other.gameObject.layer))) 
                return;
            
            OnPickedUp(other.gameObject);
        }
    }
}