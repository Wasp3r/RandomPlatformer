using RandomPlatformer.UI.Menus;
using UnityEngine;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Credits state
    /// </summary>
    public class CreditsState : BaseState
    {
        /// <summary>
        ///     Credits menu reference.
        /// </summary>
        private readonly CreditsMenu _creditsMenu;
        
        /// <summary>
        ///     Menu background reference.
        ///     We need it to enable it when we enter the main menu state.
        /// </summary>
        private readonly GameObject _menuBackground;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="creditsMenu">Credits menu reference</param>
        /// <param name="menuBackground"></param>
        public CreditsState(CreditsMenu creditsMenu, GameObject menuBackground)
        {
            _creditsMenu = creditsMenu;
            _menuBackground = menuBackground;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _creditsMenu.Enable();
            _menuBackground.SetActive(true);
            _creditsMenu.OnBack += OnCancel;
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _creditsMenu.Disable();
            _creditsMenu.OnBack -= OnCancel;
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            GameStateMachine.GoToState(State.MainMenu);
        }
    }
}