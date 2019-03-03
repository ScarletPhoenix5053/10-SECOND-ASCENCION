using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class Zipline : MonoBehaviour
    {
        private void Update()
        {
            AscendAttatched(_ascencionSpeed * Time.deltaTime);
        }

        [SerializeField]
        private float _ascencionSpeed = 1;
        [SerializeField]
        private float _launchAtTopVelocity = 30f;
        private List<PlayerController> _attatchedPlayers = new List<PlayerController>();

        /// <summary>
        /// Attatched a player to a zipline. When attatched a player will move upwards untill detatched.
        /// </summary>
        /// <param name="playerToAtattch">Player to attatch</param>
        public void AttatchPlayer(PlayerController playerToAtattch)
        {
            _attatchedPlayers.Add(playerToAtattch);
            Debug.Log("Attatched " + playerToAtattch + " to " + name);
        }
        public void DetatchPlayer(PlayerController playerToDetatch)
        {
            _attatchedPlayers.Remove(playerToDetatch);
        }

        private void AscendAttatched(float speed)
        {
            if (_attatchedPlayers == null) return;

            // Move each attatched player upwards
            // Remove players at the top of the zipline
            for (int i = 0; i < _attatchedPlayers.Count; i++)
            {
                var player = _attatchedPlayers[i];
                if (player == null) continue;

                player.transform.position += new Vector3(0, speed, 0);

                if (player.transform.position.y >= transform.position.y)
                {
                    player.Launch(new Vector2(0,_launchAtTopVelocity));
                    DetatchPlayer(player);
                }
            }
        }
    }
}
