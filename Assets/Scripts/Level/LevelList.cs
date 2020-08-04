using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "LevelList", menuName = "Game/LevelList")]
    public class LevelList : ScriptableObject
    {
        private int _currentLevel = -1;

        [SerializeField]
        private LevelControl[] ControlPrefabs;

        public void Reset()
        {
            _currentLevel = -1;
        }

        /// <summary>
        /// Returns false if all levels used
        /// </summary>
        /// <returns></returns>
        public bool ProvideNextLevel(out LevelControl prefab)
        {
            _currentLevel++;

            if (_currentLevel >= ControlPrefabs.Length)
            {
                prefab = default;
                return false;
            }

            prefab = ControlPrefabs[_currentLevel];
            return true;
        }
    }
}