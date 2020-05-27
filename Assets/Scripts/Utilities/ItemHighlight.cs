using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class ItemHighlight : MonoBehaviour
    {
        [SerializeField] private Material _outline;
        private MeshRenderer _meshRenderer;
        private bool _highlighted;

        private List<GameObject> _items = new List<GameObject>();
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void Update()
        {
            if (_items?.Count > 0 && _items[0] == gameObject)
            {
                DrawHighlight(true);
                return;
            }
            DrawHighlight(false);
        }

        public void HighlightItem(List<GameObject> items)
        {
            _items = items;
        }

        private void DrawHighlight(bool value)
        {
            if (value)
            {
                Material[] array = new Material[2];
                if (_meshRenderer)
                {
                    array[0] = _meshRenderer.materials[0];
                    array[1] = _outline;
                    _meshRenderer.materials = array;
                }

                if (_skinnedMeshRenderer)
                {
                    array[0] = _skinnedMeshRenderer.materials[0];
                    array[1] = _outline;
                    _skinnedMeshRenderer.materials = array;
                }
            }
            else
            {
                Material[] array = new Material[1];

                if (_meshRenderer)
                {
                    array[0] = _meshRenderer.materials[0];
                    _meshRenderer.materials = array;
                }

                if (_skinnedMeshRenderer)
                {
                    array[0] = _skinnedMeshRenderer.materials[0];
                    _skinnedMeshRenderer.materials = array;
                }
            }
        }
    }
}
