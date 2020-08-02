using HexCS.Mathematics;
using HexUN.Animation;
using HexUN.MonoB;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Singleton for managing time
    /// </summary>
    public class TimeManager : AMonoSingletonPersistent<TimeManager>
    {
        private int _inteprolationId;

        private float _initalFixedDeltaTime;
        private float _currentTimeScale = 1;

        public EEasingFunction EaseFunction;  
        public float EaseDuration = 1;

        public float CurrentTimeScale => _currentTimeScale;

        public void ChangeTimeScale(float scale) => ChangeTimeScale(scale, EaseDuration, EaseFunction);

        public void ChangeTimeScale(float scale, float duration, EEasingFunction ease)
        {
            // Validate
            scale = Mathf.Clamp(scale, float.Epsilon, float.MaxValue);

            //Set up interpolation
            IInterpolationToken<float[]> t = InterpolationManager.Instance.StartInterpolation(
                _inteprolationId,
                duration,
                new SInterpolation
                (
                    _currentTimeScale,
                    scale,
                    ease
                )
            );

            // set up events
            t.OnInterpolationSubscriber.Subscribe(HandleTimeScaleInterpolation);
        }

        public void ChangeTimeScaleImmediate(float scale)
        {

        }

        protected override void MonoAwake()
        {
            base.MonoAwake();
            _inteprolationId = InterpolationManager.Instance.GetUniqueId();
            _initalFixedDeltaTime = Time.fixedDeltaTime;
        }

        private void HandleTimeScaleInterpolation(float[] newTimeScale)
        {
            float scale = newTimeScale[0];
            SetTimeScale(scale);
        }

        private void SetTimeScale(float scale)
        {
            _currentTimeScale = scale;

            Time.timeScale = scale;
            Time.fixedDeltaTime = _initalFixedDeltaTime * scale;
        }
    }
}