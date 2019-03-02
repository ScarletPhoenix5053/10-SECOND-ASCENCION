using UnityEngine;
using UnityEngine.Events;

namespace Sierra.AGPW.TenSecondAscencion
{
    [RequireComponent(typeof(Collider2D))]
    public class Launchpad : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _trajectory = new Vector2(0, 0);
        [SerializeField]
        private UnityEvent _onActivation;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                _onActivation.Invoke();
                player.Launch(_trajectory);
            }
        }
    }
}