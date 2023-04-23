using System;
using UnityEngine;

namespace RandomPlatformer.Utils
{
    /// <summary>
    ///     Mono behavior singleton class.
    /// </summary>
    public class MonoBehaviorSingleton<T> : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                Debug.LogWarning($"### - {typeof(T).Name} is not initialized.");
                return default;
            }

            protected set => _instance = value;
        }

        public static bool IsInitialized => _instance != null;

        protected virtual void OnApplicationQuit()
        {
            _instance = default;
        }
    }
}