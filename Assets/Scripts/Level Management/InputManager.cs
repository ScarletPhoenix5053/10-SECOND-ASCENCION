using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class InputManager : MonoBehaviour
    {
        public PlayerController Player;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.TriggerInputJump();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                Player.TriggerInputJumpHeld();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                var input = 0f;
                if (Input.GetKey(KeyCode.A)) input -= 1;
                if (Input.GetKey(KeyCode.D)) input += 1;

                Player.TriggerInputHorizontal(input);
            }
        }
    }
}