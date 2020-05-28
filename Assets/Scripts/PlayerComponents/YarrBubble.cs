using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class YarrBubble : MonoBehaviour
    {
        private float _destructionTimer = 3f;
        private float _offset;

        private GameObject cam;

        // Start is called before the first frame update
        public void Start()
        {
            StartCoroutine(Die());
        }

        // Update is called once per frame
        public void Update()
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
            transform.position = transform.parent.position + cam.transform.TransformDirection(1.2f, 1.6f, 0);
            transform.rotation = cam.transform.rotation;
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(5f);
            //yeeees
            Destroy(gameObject);
        }
    }
}