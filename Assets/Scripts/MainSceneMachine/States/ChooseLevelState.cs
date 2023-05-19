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
        ///     Basic constructor.
        /// </summary>
        /// <param name="chooseLevelMenu">Choose level menu reference</param>
        public ChooseLevelState(ChooseLevelMenu chooseLevelMenu)
        {
            _chooseLevelMenu = chooseLevelMenu;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _chooseLevelMenu.OnBack += OnCancel;
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
    }
}