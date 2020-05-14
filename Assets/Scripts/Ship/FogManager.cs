using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

/*
namespace ScallyWags
{
    public class FogManager : MonoBehaviour
    {
        private ShipCondition _shipCondition;
        [SerializeField] private VolumeProfile _postProcessing;

        private float _maximumHeightInitial;
        private float _maximumHeightMax = 100f;
        private bool _fogOn = true;

        private float _initialSkyLux;

        void Start()
        {
            _shipCondition = GetComponent<ShipCondition>();
            _postProcessing.TryGet(out Fog fog);
            _maximumHeightInitial = fog.maximumHeight.value;
            _postProcessing.TryGet(out HDRISky sky);
            _initialSkyLux = sky.desiredLuxValue.value;
            
            ToggleFog(_fogOn);
        }

        // Update is called once per frame
        void Update()
        {
            if (_shipCondition.IsSinking() && _fogOn)
            {
                _fogOn = false;
                ToggleFog(_fogOn);
            }
        }

        private void ToggleFog(bool fogOn)
        {
            _postProcessing.TryGet(out Fog fog);
            _postProcessing.TryGet(out HDRISky sky);
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
*/