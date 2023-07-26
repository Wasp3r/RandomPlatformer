using System;
using RandomPlatformer.LevelSystem;
using RandomPlatformer.Player;
using RandomPlatformer.ScoringSystem;
using RandomPlatformer.Utils;
using UnityEngine;

namespace RandomPlatformer
{
    /// <summary>
    ///     Sound controller is responsible for playing all the SFX in the game.
    /// </summary>
    public class SoundController : MonoBehaviorSingleton<SoundController>
    {
        [SerializeField] private AudioSource _coinPickSound;
        [SerializeField] private AudioSource _heartPickSound;
        [SerializeField] private AudioSource _levelCompleteSound;
        [SerializeField] private AudioSource _walkingSound;
        [SerializeField] private AudioSource _jumpSound;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private AudioSource _looseSound;
        [SerializeField] private AudioSource _winSound;

        [SerializeField] private ScoreController _scoreController;
        [SerializeField] private LevelController _levelController;
        [SerializeField] private LivesController _livesController;

        /// <summary>
        ///     Assign the instance.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        ///     Listen to all the events to play the sounds.
        /// </summary>
        private void Start()
        {
            _scoreController.OnScoreChanged += PlayCoinPickSound;
            _livesController.OnGainedLive += PlayHeartPickSound;
            _levelController.OnLevelComplete += PlayLevelCompleteSound;
            _livesController.OnDeath += PlayLooseSound;
            _levelController.OnFinishLastLevel += PlayWinSound;
        }

        /// <summary>
        ///     Play walking sound.
        ///     We use it to play or change the pitch of the walking sound.
        /// </summary>
        /// <param name="speed">The speed of the player.</param>
        public void PlayWakingSound(float speed)
        {
            _walkingSound.pitch = speed;
        
            if (_walkingSound.isPlaying)
                return;
                
            _walkingSound.Play();
        }
        
        /// <summary>
        ///     Stop walking sound.
        /// </summary>
        public void StopWalkingSound()
        {
            _walkingSound.pitch = 0;
        }
        
        /// <summary>
        ///     Play jumping sound.
        /// </summary>
        public void PlayJumpingSound()
        {
            _jumpSound.Play();
        }
        
        /// <summary>
        ///     Sound played when the player looses a live.
        /// </summary>
        public void PlayDeathSound()
        {
            _deathSound.Play();
        }

        /// <summary>
        ///     Sound played when the player finished the last level.
        /// </summary>
        private void PlayWinSound()
        {
            StopAllSounds();
            _winSound.Play();
        }

        /// <summary>
        ///     Sound played when the player loses all lives.
        /// </summary>
        private void PlayLooseSound()
        {
            StopAllSounds();
            _looseSound.Play();
        }

        /// <summary>
        ///     Sound played when the player completes the level.
        /// </summary>
        private void PlayLevelCompleteSound()
        {
            _levelCompleteSound.Play();
        }

        /// <summary>
        ///     Sound played when the player picks up a heart.
        /// </summary>
        private void PlayHeartPickSound()
        {
            _heartPickSound.Play();
        }

        /// <summary>
        ///     Sound played when the player picks up a coin.
        /// </summary>
        /// <param name="score"></param>
        private void PlayCoinPickSound(int score)
        {
            if (score == 0)
                return;
            
            _coinPickSound.Play();
        }

        /// <summary>
        ///     Stops all the sounds.
        /// </summary>
        private void StopAllSounds()
        {
            _coinPickSound.Stop();
            _heartPickSound.Stop();
            _levelCompleteSound.Stop();
            _walkingSound.Stop();
            _jumpSound.Stop();
            _deathSound.Stop();
            _looseSound.Stop();
            _winSound.Stop();
        }
    }
}