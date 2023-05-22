using System;
using RandomPlatformer.MainSceneMachine;
using RandomPlatformer.MainSceneMachine.States;
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
        ///     Level controller to get the current level.
        /// </summary>
        private LevelController _levelController;

        /// <summary>
        ///     Is the timer enabled?
        /// </summary>
        private bool _enabled;

        /// <summary>
        ///     Current level time limit.
        /// </summary>
        private float _timeLimit;

        /// <summary>
        ///     Is the class initialized?
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        ///     Time left to the end of the level.
        /// </summary>
        public float TimeLeft => _timeLimit;
        
        /// <summary>
        ///     Event to notify the listeners that the time left has changed.
        /// </summary>
        public event Action<float> OnTimeLeftChanged;

        /// <summary>
        ///     Listen to level loaded event to get the time limit.
        /// </summary>
        private void OnEnable()
        {
            Initialize();
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
                GameStateMachine.Instance.GoToState(State.Result);
            }
            
            OnTimeLeftChanged?.Invoke(_timeLimit);
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

        private void Initialize()
        {
            if (_isInitialized)
                return;
            
            _isInitialized = true;
            _levelController = GameStateMachine.Instance.LevelController;
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