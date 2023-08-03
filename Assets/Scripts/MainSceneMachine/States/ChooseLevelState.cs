using RandomPlatformer.LevelSystem;
using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Game choose level state
    /// </summary>
    public class ChooseLevelState : BaseState
    {
        /// <summary>
        ///     Choose level menu reference.
        /// </summary>
        private readonly ChooseLevelMenu _chooseLevelMenu;
        
        /// <summary>
        ///     Level controller reference.
        /// </summary>
        private readonly LevelController _levelController;
        
        /// <summary>
        ///     Menu background reference.
        ///     We need it to enable it when we enter the main menu state.
        /// </summary>
        private readonly GameObject _menuBackground;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="chooseLevelMenu">Choose level menu reference</param>
        /// <param name="levelController">Level controller reference</param>
        /// <param name="menuBackground"></param>
        public ChooseLevelState(ChooseLevelMenu chooseLevelMenu, LevelController levelController,
            GameObject menuBackground)
        {
            _chooseLevelMenu = chooseLevelMenu;
            _levelController = levelController;
            _menuBackground = menuBackground;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _chooseLevelMenu.OnBack += OnCancel;
            _chooseLevelMenu.OnStateLevel += OnLevelSelected;
            _chooseLevelMenu.Enable();
            _menuBackground.SetActive(true);
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _chooseLevelMenu.OnBack -= OnCancel;
            _chooseLevelMenu.Disable();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            GameStateMachine.GoToState(State.MainMenu);
        }

        /// <summary>
        ///     Handles level selection.
        /// </summary>
        /// <param name="levelIndex">Level index</param>
        private void OnLevelSelected(int levelIndex)
        {
            _levelController.SelectStartingLevel(levelIndex);
            GameStateMachine.GoToState(State.GameActive);
        }
    }
}