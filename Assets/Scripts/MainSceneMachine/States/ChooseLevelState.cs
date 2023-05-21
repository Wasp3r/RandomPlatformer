using RandomPlatformer.LevelSystem;
using RandomPlatformer.UI.Menus;

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
        ///     Basic constructor.
        /// </summary>
        /// <param name="chooseLevelMenu">Choose level menu reference</param>
        /// <param name="levelController">Level controller reference</param>
        public ChooseLevelState(ChooseLevelMenu chooseLevelMenu, LevelController levelController)
        {
            _chooseLevelMenu = chooseLevelMenu;
            _levelController = levelController;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _chooseLevelMenu.OnBack += OnCancel;
            _chooseLevelMenu.OnStateLevel += OnLevelSelected;
            _chooseLevelMenu.Enable();
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