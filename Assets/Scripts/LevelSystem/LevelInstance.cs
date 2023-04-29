using System;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RandomPlatformer.LevelSystem
{
    /// <summary>
    ///     This class is responsible for the level instance.
    ///     It's used to store the level data such as: level number, time and scene reference.
    /// </summary>
    [CreateAssetMenu(fileName = "Levels/LevelInstance_", menuName = "RandomPlatformer/LevelInstance", order = 0)]
    public class LevelInstance : ScriptableObject
    {
        /// <summary>
        ///     The level number.
        /// </summary>
        public int LevelNumber = 1;
        
        /// <summary>
        ///     Time to finish the level.
        /// </summary>
        public float Time = 300f;

        /// <summary>
        ///     The scene path.
        /// </summary>
        public string ScenePath;

#if UNITY_EDITOR
        /// <summary>
        ///     The scene reference.
        /// </summary>
        public SceneAsset SceneReference;

        /// <summary>
        ///     Validate the scene reference.
        /// </summary>
        private void OnValidate()
        {
            if (SceneReference == null) 
                return;

            ScenePath = AssetDatabase.GetAssetPath(SceneReference);
        }
#endif
    }
}