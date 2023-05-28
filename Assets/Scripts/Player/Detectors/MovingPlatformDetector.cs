using System;
using UnityEngine;

namespace RandomPlatformer.Player.Detectors
{
    /// <summary>
    ///     This class is controlling the player's interaction with moving platforms.
    ///     We need it to be able to move with the platform.
    /// </summary>
    public class MovingPlatformDetector : MonoBehaviour
    {
        /// <summary>
        ///     When the player collides with the platform, we set the platform as the parent of the player.
        ///     We need to do this to be able to move with the platform.
        /// </summary>
        /// <param name="other">Other collider.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("MovingPlatform"))
                return;
            
            transform.SetParent(other.transform);
        }
        
        /// <summary>
        ///     When the player stops colliding with the platform, we set the parent to null.
        ///     We need to do this to stop moving with the platform.
        /// </summary>
        /// <param name="other">Other collider.</param>
        private void OnCollisionExit2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("MovingPlatform"))
                return;
            
            transform.SetParent(null);
        }
    }
}