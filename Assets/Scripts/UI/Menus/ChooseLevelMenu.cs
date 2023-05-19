using System;
using System.Collections;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.UI.Components;
using RandomPlatformer.UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     This class is responsible for the choose level menu.
    ///     It is used to choose the level to play. User can only choose from unlocked levels.
    /// </summary>
    public class ChooseLevelMenu : UIFadable
    {
        /// <summary>
        ///     Level controller reference.
        /// </summary>
        [SerializeField] private LevelController _levelController;

        /// <summary>
        ///     Controller support reference.
        /// </summary>
        [SerializeField] private UIButtonListControllerSupport _controllerSupport;

        /// <summary>
        ///     Level button container.
        /// </summary>
        [SerializeField] private VerticalLayoutGroup _container;

        /// <summary>
        ///     Level button prefab.
        /// </summary>
        [SerializeField] private LevelButton _levelButtonPrefab;

        /// <summary>
        ///     Back button reference.
        /// </summary>
        [SerializeField] private Button _backButton;

        /// <summary>
        ///     Removes the references to the level buttons.
        /// </summary>
        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(BackButtonClicked);
        }

        /// <summary>
        ///     Enable the menu and populate the level buttons.
        /// </summary>
        public override void Enable()
        {
            base.Enable();
            _backButton.onClick.AddListener(BackButtonClicked);
            PopulateLevelButtons();
            StartCoroutine(InitializeControllerIndicator());
        }

        /// <summary>
        ///     Creates the level buttons.
        /// </summary>
        private void PopulateLevelButtons()
        {
            var unlockedLevels = _levelController.UnlockedLevels;
            if (_container.transform.childCount >= unlockedLevels)
                return;
            
            ClearLevelButtons();
            for (var i = 0; i < _levelController.UnlockedLevels; i++)
            {
                var levelButton = Instantiate(_levelButtonPrefab, _container.transform);
                levelButton.Initialize(i);
                levelButton.OnLevelButtonClicked += OnLevelClicked;
            }
        }

        /// <summary>
        ///     Level button click event.
        ///     We use it to load the level.
        /// </summary>
        /// <param name="levelIndex">Level index.</param>
        private void OnLevelClicked(int levelIndex)
        {
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Active);
            _levelController.OpenLevel(levelIndex);
        }

        /// <summary>
        ///     Removes all the buttons from the container.
        ///     We need it to reinitialize it when the user unlocks a new level.
        /// </summary>
        private void ClearLevelButtons()
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        ///     Goes back to the main menu.
        /// </summary>
        private void BackButtonClicked()
        {
            Disable();
            GameStateController.Instance.UpdateGameState(GameState.Menu);
        }
        
        /// <summary>
        ///     Initializes the controller indicator.
        ///     We have to wait because buttons in the layout group are not aligned at the start.
        /// </summary>
        private IEnumerator InitializeControllerIndicator()
        {
            yield return new WaitForSeconds(0.1f);
            _controllerSupport.Initialize();
        }
    }
}