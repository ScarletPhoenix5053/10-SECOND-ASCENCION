using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class InputManager : MonoBehaviour
    {
        public PlayerController PlayerOne;
        public PlayerController PlayerTwo;

        private ControllerInputGroup ControllerOneInput = new ControllerInputGroup(KeyCode3)

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerOne.TriggerInputJump();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PlayerOne.TriggerInputJumpHeld();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                var input = 0f;
                if (Input.GetKey(KeyCode.A)) input -= 1;
                if (Input.GetKey(KeyCode.D)) input += 1;

                PlayerOne.TriggerInputHorizontal(input);
            }
        }
    }
    public interface IInputGroup
    {
        bool GetJump();
        bool GetJumpHeld();
        bool GetThrow();

        float GetMotionHorizontal();
        float GetMotionVertical();

        Vector2 GetAimDir();
    }
    public struct KeyboardInputGroup
    {

    }
    public struct ControllerInputGroup : IInputGroup
    {
        public ControllerInputGroup(KeyCode keyJump, KeyCode keyThrow)
        {
            _keyThrow = keyJump;
            _keyJump = keyThrow;
        }

        private KeyCode _keyThrow;
        private KeyCode _keyJump;

        public bool GetJump() => Input.GetKeyDown(_keyJump);
        public bool GetJumpHeld() => Input.GetKey(_keyJump);
        public bool GetThrow() => Input.GetKey(_keyThrow);

        public float GetMotionHorizontal()
        {
            return Input.GetAxisRaw("Horizontal");
        }
        public float GetMotionVertical()
        {
            return Input.GetAxisRaw("Vertical");
        }

        public Vector2 GetAimDir()
        {
            return new Vector2(Input.GetAxis("AimHor"), Input.GetAxis("AimVert")); 
        }
    }
}