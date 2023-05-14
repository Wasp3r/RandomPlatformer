using System;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.LevelSystem
{
    /// <summary>
    ///     Time controller class. It's responsible for measuring the time of the level.
    /// </summary>
    public class TimeController : MonoBehaviour
    {
        /// <summary>
        ///     GUI controller to update the time.
        /// </summary>
        [SerializeField] private GUIController _guiController;
        
        /// <summary>
        ///     Level controller to get the current level.
        /// </summary>
        [SerializeField] private LevelController _levelController;

        /// <summary>
        ///     Is the timer enabled?
        /// </summary>
        private bool _enabled;

        /// <summary>
        ///     Current level time limit.
        /// </summary>
        private float _timeLimit;

        /// <summary>
        ///     Time left to the end of the level.
        /// </summary>
        public float TimeLeft => _timeLimit;

        /// <summary>
        ///     Listen to level loaded event to get the time limit.
        /// </summary>
        private void OnEnable()
        {
            _levelController.OnLevelLoaded += OnLevelLoaded;
        }

        /// <summary>
        ///     Stop listening to level loaded event.
        /// </summary>
        private void OnDisable()
        {
            _levelController.OnLevelLoaded -= OnLevelLoaded;
        }

        /// <summary>
        ///     Update the timer.
        /// </summary>
        private void Update()
        {
            if (!_enabled)
                return;
            
            _timeLimit -= Time.deltaTime;
            
            if (_timeLimit <= 0)
            {
                _enabled = false;
                _timeLimit = 0;
                GameStateController.Instance.UpdateGameState(GameState.Failed);
            }
            
            _guiController.UpdateTimeLeft(_timeLimit);
        }

        /// <summary>
        ///     Set the time limit to the current level time limit.
        /// </summary>
        /// <param name="loadedLevel">Loaded level.</param>
        private void OnLevelLoaded(LevelInstance loadedLevel)
        {
            _enabled = true;
            _timeLimit = loadedLevel.Time;
        }

#if UNITY_EDITOR
        [ContextMenu("Set time to one")]
        private void SetTimeToOne()
        {
            _timeLimit = 1;
        }
#endif
    }
}