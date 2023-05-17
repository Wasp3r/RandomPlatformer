namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Base state class
    /// </summary>
    public abstract class BaseState
    {
        /// <summary>
        ///     Game state machine reference.
        /// </summary>
        protected GameStateMachine GameStateMachine;

        /// <summary>
        ///     Initialization method used to assign game state machine.
        /// </summary>
        /// <param name="gameStateMachine">Game state machine</param>
        public virtual void Initialize(GameStateMachine gameStateMachine)
        {
            GameStateMachine = gameStateMachine;
        }

        /// <summary>
        ///     Called when enters this state.
        /// </summary>
        public abstract void OnEnterState();

        /// <summary>
        ///     Called when exiting this state.
        /// </summary>
        public abstract void OnExitState();

        /// <summary>
        ///     Method to be called when cancel action is performed.
        /// </summary>
        public abstract void OnCancel();
    }
}