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

        void Start()
        {
            _shipCondition = GetComponent<ShipCondition>();
            _postProcessing = FindObjectOfType<Volume>();
            _postProcessing.profile.TryGet(out Fog fog);
            _maximumHeightInitial = fog.maximumHeight.value;
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
            if (fogOn)
            {
                DOTween.To(x => fog.maximumHeight.value = x, _maximumHeightInitial, _maximumHeightMax, 5f);
            }
            else
            {
                DOTween.To(x => fog.maximumHeight.value = x, _maximumHeightMax, _maximumHeightInitial, 5f);
            }
        }
    }
}