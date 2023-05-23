using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI.Generic
{
    /// <summary>
    ///     This class is responsible for the fadeable UI elements.
    /// </summary>
    public class UIFadable : MonoBehaviour
    {
        /// <summary>
        ///     The background image.
        /// </summary>
        [SerializeField] private Image _backgroundImage;

        /// <summary>
        ///     Fades in the UI element.
        /// </summary>
        public virtual void Enable()
        {
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0.7f);
            Open();
        }
        
        /// <summary>
        ///     Fades out the UI element.
        /// </summary>
        public virtual void Disable()
        {
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, 0f);
            Close();
        }

        /// <summary>
        ///     Enables the UI element.
        /// </summary>
        private void Open()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        ///     Disables the UI element.
        /// </summary>
        private void Close()
        {
            gameObject.SetActive(false);
        }
    }
}