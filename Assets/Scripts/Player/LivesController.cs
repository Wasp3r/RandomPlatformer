using System;
using UnityEngine;

namespace RandomPlatformer.Player
{
    /// <summary>
    ///     This class is responsible for managing the player's lives.
    /// </summary>
    public class LivesController : MonoBehaviour
    {
        /// <summary>
        ///     The number of lives the player has.
        /// </summary>
        [SerializeField] private int _defaultLives = 5;

        /// <summary>
        ///     The current number of lives the player has.
        /// </summary>
        private int _currentLives;

        /// <summary>
        ///     Triggered when the player loses all of their lives.
        /// </summary>
        public event Action OnAllLivesLost;
        
        /// <summary>
        ///     Triggered when the player's lives change.
        /// </summary>
        public event Action<int> OnLivesChanged;

        /// <summary>
        ///     Triggered when the player loses a live.
        /// </summary>
        public event Action OnLostLive;

        /// <summary>
        ///     The number of lives the player has.
        /// </summary>
        public int Lives => _currentLives;

        /// <summary>
        ///     Sets the current lives to the default value.
        /// </summary>
        private void OnEnable()
        {
            _currentLives = _defaultLives;
        }

        /// <summary>
        ///     Takes away a live from the player and triggers the OnAllLivesLost event if the player has no lives left.
        /// </summary>
        [ContextMenu("Love live")]
        public void LoseLive()
        {
            _currentLives--;
            OnLostLive?.Invoke();
            OnLivesChanged?.Invoke(_currentLives);
            
            if (_currentLives > 0)
                return;
            
            ResetLives();
            OnAllLivesLost?.Invoke();
        }

        /// <summary>
        ///     Adds a live to the player.
        /// </summary>
        [ContextMenu("Add live")]
        public void AddLive()
        {
            _currentLives++;
            OnLivesChanged?.Invoke(_currentLives);
        }

        /// <summary>
        ///     Resets the player's lives to the default value.
        /// </summary>
        public void ResetLives()
        {
            _currentLives = _defaultLives;
            OnLivesChanged?.Invoke(_currentLives);
        }
    }
}