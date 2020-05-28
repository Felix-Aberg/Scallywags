using UnityEngine;

namespace ScallyWags
{
    [System.Serializable]

    public class Emote
    {
        private float _delay = 2f;
        private float _timer;
        private AudioSourcePoolManager _audioSourcePoolManager;
        private SimpleAudioEvent _emoteAudio;
        private AnimationController _animation;
        [SerializeField] private GameObject _yarrBubble;

        public void Init(SimpleAudioEvent emoteAudio, AudioSourcePoolManager audioSourcePoolManager, AnimationController animationController)
        {
            _audioSourcePoolManager = audioSourcePoolManager;
            _emoteAudio = emoteAudio;
            _animation = animationController;
        }

        // Update is called once per frame
        public void Tick(bool emoteDown, Transform transform)
        {
            _timer += Time.deltaTime;

            if (emoteDown)
            {
                if (_timer > _delay)
                {
                    //Execute yarr!
                    _timer = 0;
                    _audioSourcePoolManager.PlayAudioEvent(_emoteAudio, transform.position);
                    _animation.Emote();
                    Object.Instantiate(_yarrBubble, transform, transform);
                }
            }
        }
    }
}

