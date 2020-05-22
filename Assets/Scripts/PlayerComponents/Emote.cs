using UnityEngine;

namespace ScallyWags
{
    public class Emote
    {
        private float _delay = 2f;
        private float _timer;
        private AudioSourcePoolManager _audioSourcePoolManager;
        private SimpleAudioEvent _emoteAudio;
        private AnimationController _animation;

        public void Init(SimpleAudioEvent emoteAudio, AudioSourcePoolManager audioSourcePoolManager, AnimationController animationController)
        {
            _audioSourcePoolManager = audioSourcePoolManager;
            _emoteAudio = emoteAudio;
            _animation = animationController;
        }

        // Update is called once per frame
        public void Tick(bool emoteDown)
        {
            _timer += Time.deltaTime;

            if (emoteDown)
            {
                if (_timer > _delay)
                {
                    _timer = 0;
                    _audioSourcePoolManager.PlayAudioEvent(_emoteAudio);
                    _animation.Emote();
                }
            }
        }
    }
}

