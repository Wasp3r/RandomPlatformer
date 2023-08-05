using RandomPlatformer.LevelSystem;
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
        ///     Level controller reference.
        /// </summary>
        private readonly LevelController _levelController;
        
        /// <summary>
        ///     Camera controller reference.
        ///     We need it to be able to reset the camera position.
        /// </summary>
        private readonly CameraController _cameraController;
        
        /// <summary>
        ///     Menu background reference.
        ///     We need it to enable it when we enter the main menu state.
        /// </summary>
        private readonly GameObject _menuBackground;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="mainMenu">Main menu reference</param>
        /// <param name="levelController">Level controller reference</param>
        /// <param name="cameraController">Camera controller reference</param>
        /// <param name="menuBackground"></param>
        public MainMenuState(MainMenu mainMenu, LevelController levelController, CameraController cameraController,
            GameObject menuBackground)
        {
            _mainMenu = mainMenu;
            _levelController = levelController;
            _cameraController = cameraController;
            _menuBackground = menuBackground;
        }
        
        /// <inheridoc/>
        public override void OnEnterState()
        {
            _mainMenu.Enable();
            _menuBackground.SetActive(true);
            _mainMenu.OnStartGame += GoToGame;
            _mainMenu.OnLeaderboard += GoToLeaderboard;
            _mainMenu.OnCredits += GoToCredits;
            _mainMenu.OnChooseLevel += GoToChooseLevel;
            _mainMenu.OnExitGame += ExitGame;
            _levelController.SelectStartingLevel(0);
            _cameraController.ResetCameraPosition();
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _mainMenu.OnStartGame -= GoToGame;
            _mainMenu.OnLeaderboard -= GoToLeaderboard;
            _mainMenu.OnCredits -= GoToCredits;
            _mainMenu.OnChooseLevel -= GoToChooseLevel;
            _mainMenu.OnExitGame -= ExitGame;
            _mainMenu.Disable();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            // You can only exit the game when not on WebGL.
#if !UNITY_WEBGL
            ExitGame();
#endif
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
        
        /// <summary>
        ///     Goes to the credits state.
        /// </summary>
        private void GoToCredits()
        {
            GameStateMachine.GoToState(State.Credits);
        }
    }
}