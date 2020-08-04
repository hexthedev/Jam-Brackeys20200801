using HexCS.Mathematics;
using HexUN.Animation;
using HexUN.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class LevelSwitchHandler : MonoBehaviour
    {
        private int _interpId;
        public LevelList _levels;
        public float transitionDuration;
        public EEasingFunction ease;

        public StringSoEvent _onLevelStart;

        private LevelControl _currentLevel;

        private void Awake()
        {
            _interpId = InterpolationManager.Instance.GetUniqueId();
        }

        public void Start()
        {
            _levels.Reset();

            if (_levels.ProvideNextLevel(out LevelControl ctrl))
            {
                LevelControl inst = Instantiate(ctrl, transform);
                inst.transform.position = new Vector3(0, 0, 0);
                _currentLevel = inst;
            }
        }

        [ContextMenu("NextLevel")]
        public void SpawnNextLevel()
        {
            if(_levels.ProvideNextLevel(out LevelControl ctrl))
            {
                LevelControl inst = Instantiate(ctrl, transform);
                inst.transform.position = new Vector3(-8, 0, 0);

                IInterpolationToken<float[]> toke = InterpolationManager.Instance.StartInterpolation(
                    _interpId,
                    transitionDuration,
                    new SInterpolation()
                    {
                        Start = 0,
                        End = 8,
                        Ease = ease
                    }
                );

                toke.OnInterpolationSubscriber.Subscribe(
                    f =>
                    {
                        _currentLevel.transform.position = new Vector3(f[0], _currentLevel.transform.position.y, _currentLevel.transform.position.z);
                        inst.transform.position = new Vector3(f[0]-8, inst.transform.position.y, inst.transform.position.z);
                    }
                );

                toke.OnInterpolationEndSubscriber.Subscribe(
                    () =>
                    {
                        Destroy(_currentLevel.gameObject);
                        _currentLevel = inst;
                        LevelManager.Instance.RegisterAsCurrent(_currentLevel);
                        _onLevelStart.Invoke(ELevelEvent.EnterPlay.ToString());
                    }
                );
            }
        }
    }
}