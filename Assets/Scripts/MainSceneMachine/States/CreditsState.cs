using RandomPlatformer.UI.Menus;

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
        ///     Basic constructor.
        /// </summary>
        /// <param name="creditsMenu">Credits menu reference</param>
        public CreditsState(CreditsMenu creditsMenu)
        {
            _creditsMenu = creditsMenu;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _creditsMenu.Enable();
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