using RandomPlatformer.LevelSystem;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.UI.Menus;

namespace RandomPlatformer.MainSceneMachine.States
{
    /// <summary>
    ///     Game state
    /// </summary>
    public class GameActiveState : BaseState
    {
        /// <summary>
        ///     Player lives controller reference.
        /// </summary>
        private readonly LivesController _livesController;
        
        /// <summary>
        ///     Levels controller reference.
        /// </summary>
        private readonly LevelController _levelController;

        /// <summary>
        ///     Score controller reference.
        /// </summary>
        private readonly ScoreController _scoreController;
        
        /// <summary>
        ///     GUI controller reference.
        /// </summary>
        private readonly GUIController _guiController;
        
        /// <summary>
        ///     Camera controller reference.
        /// </summary>
        private readonly CameraController _cameraController;

        /// <summary>
        ///     Basic constructor.
        /// </summary>
        /// <param name="livesController">Lives controller reference</param>
        /// <param name="levelController">Levels controller reference</param>
        /// <param name="scoreController">Score controller reference</param>
        public GameActiveState(LivesController livesController, LevelController levelController, ScoreController scoreController, GUIController guiController, CameraController cameraController)
        {
            _livesController = livesController;
            _levelController = levelController;
            _scoreController = scoreController;
            _guiController = guiController;
            _cameraController = cameraController;
        }

        /// <inheridoc/>
        public override void OnEnterState()
        {
            _livesController.ResetLives();
            _scoreController.ResetScore();
            _levelController.StartGame();
            _guiController.Enable();
        }

        /// <inheridoc/>
        public override void OnExitState()
        {
            _guiController.Disable();
            _levelController.UnloadedCurrentLevel();
            _cameraController.StopFollowing();
        }

        /// <inheridoc/>
        public override void OnCancel()
        {
            GameStateMachine.GoToState(State.Pause);
        }
    }
}