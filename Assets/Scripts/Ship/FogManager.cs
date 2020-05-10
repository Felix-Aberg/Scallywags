using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace ScallyWags
{
    public class FogManager : MonoBehaviour
    {
        private ShipCondition _shipCondition;
        private Volume _postProcessing;

        private float _maximumHeightInitial;
        private float _maximumHeightMax = 150f;
        private bool _fogOn = true;

        private float _initialSkyLux;

        void Start()
        {
            _shipCondition = GetComponent<ShipCondition>();
            _postProcessing = FindObjectOfType<Volume>();
            _postProcessing.profile.TryGet(out Fog fog);
            _maximumHeightInitial = fog.maximumHeight.value;
            _postProcessing.profile.TryGet(out HDRISky sky);
            _initialSkyLux = sky.desiredLuxValue.value;
            
            ToggleFog(_fogOn);
        }

        // Update is called once per frame
        void Update()
        {
            if (_shipCondition.GetHealth() <= 0 && _fogOn)
            {
                _fogOn = false;
                ToggleFog(_fogOn);
            }
        }

        private void ToggleFog(bool fogOn)
        {
            _postProcessing.profile.TryGet(out Fog fog);
            _postProcessing.profile.TryGet(out HDRISky sky);
            if (fogOn)
            {
                DOTween.To(x => sky.desiredLuxValue.value = x, _initialSkyLux, 0, 3f).OnComplete(() =>
                    DOTween.To(x => fog.maximumHeight.value = x, _maximumHeightInitial, _maximumHeightMax, 3f));
            }
            else
            {
                DOTween.To(x => sky.desiredLuxValue.value = x, 0, _initialSkyLux, 3f).OnComplete(() => 
                    DOTween.To(x => fog.maximumHeight.value = x, _maximumHeightMax, _maximumHeightInitial, 3f));
            }
        }
    }
}