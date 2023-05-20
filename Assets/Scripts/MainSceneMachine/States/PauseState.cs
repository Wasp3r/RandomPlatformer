using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Game pause state
    /// </summary>
    public class PauseState : BaseState
    {
        /// <summary>
        ///     Pause menu reference.
        /// </summary>
        private readonly PauseMenu _pauseMenu;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="pauseMenu">Pause menu reference</param>
        public PauseState(PauseMenu pauseMenu)
        {
            _pauseMenu = pauseMenu;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _pauseMenu.Enable();
            _pauseMenu.OnGoToMenu += OnGoToMenu;
            _pauseMenu.OnResume += OnCancel;
            _pauseMenu.OnExit += OnExit;
            Time.timeScale = 0;
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            Time.timeScale = 1;
            _pauseMenu.OnGoToMenu -= OnGoToMenu;
            _pauseMenu.OnResume -= OnCancel;
            _pauseMenu.OnExit -= OnExit;
            _pauseMenu.Disable();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            GameStateMachine.GoToState(State.GameActive);
        }

        /// <summary>
        ///     Goes to main menu.
        /// </summary>
        private void OnGoToMenu()
        {
            GameStateMachine.GoToState(State.MainMenu);
        }

        /// <summary>
        ///     Handles game exit.
        /// </summary>
        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}