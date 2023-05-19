using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Game main menu state
    /// </summary>
    public class MainMenuState : BaseState
    {
        /// <summary>
        ///     Main menu reference.
        /// </summary>
        private readonly MainMenu _mainMenu;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="mainMenu">Main menu reference</param>
        public MainMenuState(MainMenu mainMenu)
        {
            _mainMenu = mainMenu;
        }
        
        /// <inheridoc/>
        public override void OnEnterState()
        {
            _mainMenu.Enable();
            _mainMenu.OnStartGame += GoToGame;
            _mainMenu.OnLeaderboard += GoToLeaderboard;
            _mainMenu.OnChooseLevel += GoToChooseLevel;
            _mainMenu.OnExitGame += ExitGame;
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _mainMenu.OnStartGame -= GoToGame;
            _mainMenu.OnLeaderboard -= GoToLeaderboard;
            _mainMenu.OnChooseLevel -= GoToChooseLevel;
            _mainMenu.OnExitGame -= ExitGame;
            _mainMenu.Disable();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            ExitGame();
        }

        /// <summary>
        ///     Exits the game.
        /// </summary>
        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        ///     Goes tp level choose state.
        /// </summary>
        private void GoToChooseLevel()
        {
            GameStateMachine.GoToState(State.ChooseLevel);
        }

        /// <summary>
        ///     Goes to the leaderboard state.
        /// </summary>
        private void GoToLeaderboard()
        {
            GameStateMachine.GoToState(State.LeaderBoard);
        }

        /// <summary>
        ///     Goes to the game state.
        /// </summary>
        private void GoToGame()
        {
            GameStateMachine.GoToState(State.GameActive);
        }
    }
}