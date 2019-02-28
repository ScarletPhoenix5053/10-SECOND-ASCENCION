using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class GrabBox : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Size);
        }

        public Vector2 Size
        {
            get { return new Vector3(_size.x, _size.y, 1.5f); }
            set { _size = value; }
        }

        [SerializeField]
        private Vector2 _size = Vector2.one;

        /// <summary>
        /// Activates this <see cref="GrabBox"/> and returns a <see cref="PlayerController"/>
        /// if it finds one.
        /// </summary>
        /// <returns></returns>
        public PlayerController GrabPlayer()
        {
            // Boxcast and check all matches for a player controller
            var parentPlayer = GetComponentInParent<PlayerController>();
            var objects =
                Physics2D.OverlapBoxAll(transform.position, Size, 0);
            foreach (Collider2D obj in objects)
            {
                var foundPlayer = obj.GetComponent<PlayerController>();
                if (foundPlayer != null)
                {
                    // return the first playercontroller found that is not the parent
                    if (parentPlayer == foundPlayer) continue;
                    return foundPlayer;
                }
            }

            // return null if nothing is found
            return null;
        }
    }
}