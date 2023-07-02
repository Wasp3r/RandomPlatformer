using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RandomPlatformer.UI
{
    public class ScrollablePositioner : MonoBehaviour
    {
        [SerializeField] private ContentSizeFitter _contentSizeFitter;
        
        private RectTransform _rectTransform;
        
        /// <summary>
        ///     Assign the rect transform.
        /// </summary>
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        /// <summary>
        ///     Update the position of the list after one frame.
        /// </summary>
        public void UpdatePosition()
        {
            _contentSizeFitter.SetLayoutVertical();
            StartCoroutine(UpdatePositionCoroutine());
        }

        /// <summary>
        ///     Update the position of the list after one frame.
        ///     We have to do it this way, because the content size fitter doesn't update the size immediately.
        /// </summary>
        private IEnumerator UpdatePositionCoroutine()
        {
            yield return new WaitUntil(() => _rectTransform.sizeDelta.y > 0);
            _rectTransform.position = new Vector3(_rectTransform.position.x, -_rectTransform.sizeDelta.y/2, 0);
        }
    }
}