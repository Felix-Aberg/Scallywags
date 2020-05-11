using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScallyWags
{
    public class FilmStudioPlayerScript : MonoBehaviour
    {
        public List<GameObject> _playerGameObjects;

        // Start is called before the first frame update
        void Start()
        {
            int i = 0;
            foreach (GameObject player in _playerGameObjects)
            {
                i++;
                player.GetComponent<Player>().Init(i);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            foreach (GameObject player in _playerGameObjects)
            {
                player.GetComponent<Player>().Tick();
            }
        }
    }
}
