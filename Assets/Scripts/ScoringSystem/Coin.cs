using UnityEngine;

namespace RandomPlatformer.ScoringSystem
{
    /// <summary>
    ///     The coin that the player can pick up.
    /// </summary>
    public class Coin : MonoBehaviour
    {
        /// <summary>
        ///     The number of points the coin is worth.
        /// </summary>
        [SerializeField] private int _points;

        /// <summary>
        ///     Picks up the coin and destroys it.
        ///     We need it to be able to pick up the coin from the outside.
        /// </summary>
        /// <returns>The number of points the coin is worth.</returns>
        public int Pick()
        {
            Destroy(gameObject);
            return _points;
        }
    }
}