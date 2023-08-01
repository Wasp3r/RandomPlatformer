using System;
using RandomPlatformer.UI.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Menus
{
    /// <summary>
    ///     This class is responsible for the credits menu.
    /// </summary>
    public class CreditsMenu : UIFadable
    {
        /// <summary>
        ///     World textures button reference.
        ///     It's responsible for redirecting the user to the world textures creator.
        /// </summary>
        [SerializeField] private Button _worldTexturesButton;
        
        /// <summary>
        ///     Player sprites button reference.
        ///     It's responsible for redirecting the user to the player sprites creator.
        /// </summary>
        [SerializeField] private Button _playerSpritesButton;
        
        /// <summary>
        ///     UI textures button reference.
        ///     It's responsible for redirecting the user to the UI textures creator.
        /// </summary>
        [SerializeField] private Button _uiTexturesButton;
        
        /// <summary>
        ///     Music theme button reference.
        ///     It's responsible for redirecting the user to the music theme creator.
        /// </summary>
        [SerializeField] private Button _musicThemeButton;
        
        /// <summary>
        ///     Rest button reference.
        ///     It's responsible for redirecting to Vivid Gray's website.
        /// </summary>
        [SerializeField] private Button _restButton;
        
        /// <summary>
        ///     Back button reference.
        /// </summary>
        [SerializeField] private Button _backButton;
        
        /// <summary>
        ///     Event to invoke when the back button is clicked.
        /// </summary>
        public event Action OnBack;
        
        private const string WorldTexturesUrl = "https://analogstudios.itch.io/four-seasons-platformer-sprites";
        private const string PlayerSpritesUrl = "https://free-game-assets.itch.io/free-tiny-hero-sprites-pixel-art";
        private const string UITexturesUrl = "https://penzilla.itch.io/handdrawn-vector-icon-pack";
        private const string MusicThemeUrl = "https://timbeek.itch.io/royalty-free-music-pack-volume-2";
        private const string RestUrl = "https://vividgray.itch.io/";
        
        /// <summary>
        ///     Enable the menu.
        /// </summary>
        public override void Enable()
        {
            base.Enable();
            RegisterCreditsButtons();
            _backButton.onClick.AddListener(BackButtonClicked);
        }
        
        /// <summary>
        ///     Removes the references to the level buttons.
        /// </summary>
        private void OnDisable()
        {
            UnregisterCreditsButtons();
            _backButton.onClick.RemoveListener(BackButtonClicked);
        }

        /// <summary>
        ///     Goes back to the main menu.
        /// </summary>
        private void BackButtonClicked()
        {
            OnBack?.Invoke();
        }

        private void RegisterCreditsButtons()
        {
            _worldTexturesButton.onClick.AddListener(WorldTexturesButtonClicked);
            _playerSpritesButton.onClick.AddListener(PlayerSpritesButtonClicked);
            _uiTexturesButton.onClick.AddListener(UITexturesButtonClicked);
            _musicThemeButton.onClick.AddListener(MusicThemeButtonClicked);
            _restButton.onClick.AddListener(RestButtonClicked);
        }
        
        private void UnregisterCreditsButtons()
        {
            _worldTexturesButton.onClick.RemoveListener(WorldTexturesButtonClicked);
            _playerSpritesButton.onClick.RemoveListener(PlayerSpritesButtonClicked);
            _uiTexturesButton.onClick.RemoveListener(UITexturesButtonClicked);
            _musicThemeButton.onClick.RemoveListener(MusicThemeButtonClicked);
            _restButton.onClick.RemoveListener(RestButtonClicked);
        }

        private void RestButtonClicked()
        {
            Application.OpenURL(RestUrl);
        }

        private void MusicThemeButtonClicked()
        {
            Application.OpenURL(MusicThemeUrl);
        }

        private void UITexturesButtonClicked()
        {
            Application.OpenURL(UITexturesUrl);
        }

        private void PlayerSpritesButtonClicked()
        {
            Application.OpenURL(PlayerSpritesUrl);
        }

        private void WorldTexturesButtonClicked()
        {
            Application.OpenURL(WorldTexturesUrl);
        }
    }
}