using System;
using System.Collections.Generic;
using RandomPlatformer.MainSceneMachine;
using RandomPlatformer.MainSceneMachine.States;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RandomPlatformer.LevelSystem
{
    /// <summary>
    ///     This script is responsible for controlling the levels.
    ///     It's used to load and unload levels, as well as to move to the next level.
    /// </summary>
    public class LevelController : MonoBehaviour
    {
        /// <summary>
        ///     List of levels.
        /// </summary>
        [SerializeField] private List<LevelInstance> _levels;
        
        private const string UnlockedLevelsKey = "UnlockedLevels";
        
        /// <summary>
        ///     Current level index.
        /// </summary>
        private int _currentLevelIndex;

        /// <summary>
        ///     Starting level index.
        /// </summary>
        private int _startingLevelIndex;
        
        /// <summary>
        ///     Current level scene path.
        /// </summary>
        private string _currentLevelScenePath;

        /// <summary>
        ///     Event that is called when level is loaded.
        /// </summary>
        public event Action<LevelInstance> OnLevelLoaded;

        /// <summary>
        ///     Event triggered when the level is completed.
        /// </summary>
        public Action OnLevelComplete;

        /// <summary>
        ///     Event triggered when the last level is completed.
        /// </summary>
        public Action OnFinishLastLevel;

        /// <summary>
        ///     Unlocked levels count.
        /// </summary>
        public int UnlockedLevels { get; private set; }

        /// <summary>
        ///     Load unlocked levels count.
        /// </summary>
        private void OnEnable()
        {
            UnlockedLevels = PlayerPrefs.GetInt(UnlockedLevelsKey, 1);
        }

        /// <summary>
        ///     Starting the game from the <see cref="_startingLevelIndex"/> level.
        /// </summary>
        public void StartGame()
        {
            OpenLevel(_startingLevelIndex);
        }

        /// <summary>
        ///     Opens level with specified index.
        /// </summary>
        /// <param name="index">Level index.</param>
        private void OpenLevel(int index)
        {
            if (_levels.Count <= index)
            {
                GameStateMachine.Instance.GoToState(State.Result);
                OnFinishLastLevel?.Invoke();
                return;
            }
            
            var level = _levels[index];
            _currentLevelIndex = index;
            _currentLevelScenePath = level.ScenePath;
            Time.timeScale = 0;
            SceneManager.LoadSceneAsync(level.ScenePath, LoadSceneMode.Additive).completed += OnSceneLoaded;
        }
        
        /// <summary>
        ///     Sets the starting level index.
        ///     We need it to be able to set starting level without loading it.
        /// </summary>
        /// <param name="levelIndex">Level index.</param>
        public void SelectStartingLevel(int levelIndex)
        {
            _startingLevelIndex = levelIndex;
        }

        /// <summary>
        ///     Tries to go to the next level.
        ///     If there is no next level, logs an error.
        /// </summary>
        [ContextMenu("Go to next level")]
        public void GoToNextLevel()
        {
            _currentLevelIndex++;
            if (_levels.Count > _currentLevelIndex)
            {
                UnlockedLevels = _currentLevelIndex + 1;
                SaveUnlockedLevels();
            }

            Time.timeScale = 0;
            OnLevelComplete?.Invoke();
            SceneManager.UnloadSceneAsync(_currentLevelScenePath).completed += OnSceneUnloaded;
        }
        
        /// <summary>
        ///     Unloads current level.
        /// </summary>
        public void UnloadedCurrentLevel()
        {
            Debug.Log("### - Unloading current level");
            if (SceneManager.sceneCount == 1)
                return;
            SceneManager.UnloadSceneAsync(_currentLevelScenePath);
        }

        /// <summary>
        ///     Called when scene unload operation is completed.
        /// </summary>
        /// <param name="operationResult">Operation result.</param>
        private void OnSceneUnloaded(AsyncOperation operationResult)
        {
            Time.timeScale = 1;
            if (!operationResult.isDone)
            {
                Debug.LogError("### - Scene unload failed!");
                return;
            }
            
            OpenLevel(_currentLevelIndex);
        }

        /// <summary>
        ///     Called when scene load operation is completed.
        /// </summary>
        /// <param name="operationResult">Operation result.</param>
        private void OnSceneLoaded(AsyncOperation operationResult)
        {
            Time.timeScale = 1;
            if (!operationResult.isDone)
            {
                Debug.LogError("### - Scene load failed!");
                return;
            }
            
            OnLevelLoaded?.Invoke(_levels[_currentLevelIndex]);
        }
        
        /// <summary>
        ///     Save unlocked levels count.
        /// </summary>
        private void SaveUnlockedLevels()
        {
            PlayerPrefs.SetInt(UnlockedLevelsKey, UnlockedLevels);
        }

#if UNITY_EDITOR

        /// <summary>
        ///     Reset unlocked levels count.
        /// </summary>
        [ContextMenu("Reset unlocked levels")]
        private void ResetUnlockedLevels()
        {
            PlayerPrefs.DeleteKey(UnlockedLevelsKey);
        }
        
#endif
    }
}