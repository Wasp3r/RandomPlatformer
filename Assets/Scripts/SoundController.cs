using RandomPlatformer.LevelSystem;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using UnityEngine;

namespace RandomPlatformer
{
    /// <summary>
    ///     Sound controller is responsible for playing all the SFX in the game.
    /// </summary>
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _coinPickSound;
        [SerializeField] private AudioSource _heartPickSound;
        [SerializeField] private AudioSource _levelCompleteSound;
        [SerializeField] private AudioSource _walkingSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private AudioSource _looseSound;
        [SerializeField] private AudioSource _winSound;

        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private LivesController _livesController;
        
        private void StopAllSounds()
        {
            _coinPickSound.Stop();
            _heartPickSound.Stop();
            _levelCompleteSound.Stop();
            _walkingSound.Stop();
            _deathSound.Stop();
            _looseSound.Stop();
            _winSound.Stop();
        }
    }
}