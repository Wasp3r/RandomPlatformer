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
        ///     The time it takes to fade in/out.
        /// </summary>
        [SerializeField] private float _fadeTime;
        
        /// <summary>
        ///     The coroutine that fades the UI element.
        /// </summary>
        private Coroutine _fadeCoroutine;
        
        /// <summary>
        ///     Fades in the UI element.
        /// </summary>
        public void FadeIn()
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);
            
            Enable();
            _fadeCoroutine = StartCoroutine(FadeCoroutine(0.3f));
        }
        
        /// <summary>
        ///     Fades out the UI element.
        /// </summary>
        public void FadeOut()
        {
            if (_fadeCoroutine != null)
                StopCoroutine(_fadeCoroutine);
            
            _fadeCoroutine = StartCoroutine(FadeCoroutine(0, Disable));
        }

        /// <summary>
        ///     Enables the UI element.
        /// </summary>
        private void Enable()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        ///     Disables the UI element.
        /// </summary>
        private void Disable()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     The coroutine that fades the UI element.
        /// </summary>
        /// <param name="targetAlpha">Target alpha value.</param>
        /// <param name="callback">Callback to invoke when the fade is done.</param>
        private IEnumerator FadeCoroutine(float targetAlpha, Action callback = null)
        {
            var startAlpha = _backgroundImage.color.a;
            var time = 0f;
            
            while (time < _fadeTime)
            {
                time += Time.unscaledDeltaTime;
                var alpha = Mathf.Lerp(startAlpha, targetAlpha, time / _fadeTime);
                _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, alpha);
                yield return null;
            }
            
            callback?.Invoke();
        }
    }
}