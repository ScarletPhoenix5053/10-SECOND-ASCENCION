using System;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class InputManager : MonoBehaviour
    {
        private void Awake()
        {
            ControllerInputAll = new ControllerInputGroup(PlayerOneControllerIndex);
        }
        private void Start()
        {
            LogControllerCountAndNames();
        }
        private void Update()
        {
            if (ControllerInputAll.GetJump())
            {
                PlayerOne.TriggerInputJump();
            }
            if (ControllerInputAll.GetJumpHeld())
            {
                PlayerOne.TriggerInputJumpHeld();
            }
            if (ControllerInputAll.GetMotionHorizontal() <= -0.1 ||
                ControllerInputAll.GetMotionHorizontal() >= 0.1)
            {
                PlayerOne.TriggerInputHorizontal(
                    Mathf.Sign(
                        ControllerInputAll.GetMotionHorizontal()
                        ));
            }

            Array values = Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode code in values)
            {
                if (Input.GetKeyDown(code)) { print(Enum.GetName(typeof(KeyCode), code)); }
            }
        }

        public PlayerController PlayerOne;
        public int PlayerOneControllerIndex;
        public PlayerController PlayerTwo;
        public int PlayerTwoControllerIndex = 2;

        private ControllerInputGroup ControllerInputAll;

        private void LogControllerCountAndNames()
        {
            var padNames = Input.GetJoystickNames();

            Debug.Log("Found " + padNames.Length + " controllers");
            foreach (string pad in padNames)
            {
                Debug.Log(pad);
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
        /// <summary>
        /// Instantiate a ControllerInputGroup which uses the controller at the specified index.
        /// </summary>
        /// <param name="controllerNum">An integer from 0-4. Speciifes which controller to read input from. Check all controllers if 0 is passed.</param>
        public ControllerInputGroup(int controllerIndex)
        {
            if (controllerIndex == 0)
            {
                _axisLeftStickX = "JoystickAnyLeftStickX";
                _axisLeftStickY = "JoystickAnyLeftStickY";

                _keyThrow = KeyCode.JoystickButton3;
                _keyJump = KeyCode.JoystickButton0;
            }
            else if (controllerIndex > 0 && controllerIndex <= 4)
            {
                _axisLeftStickX = "Joystick" + controllerIndex + "LeftStickX";
                _axisLeftStickY = "Joystick" + controllerIndex + "LeftStickY";

                _keyThrow = (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + controllerIndex + "Button3");
                _keyJump = (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + controllerIndex + "Button0");
                /*
                Debug.Log("Assigning _keyThrow as " + _keyThrow);
                Debug.Log("Assigning _keyJump as " + _keyJump);
                */
            }
            else
            {
                throw new ArgumentException("This index must be an integer between 0 and 4.", "controllerIndex");
            }
        }

        private readonly KeyCode _keyThrow;
        private readonly KeyCode _keyJump;
        private readonly string _axisLeftStickX;
        private readonly string _axisLeftStickY;

        public bool GetJump() => Input.GetKeyDown(_keyJump);
        public bool GetJumpHeld() => Input.GetKey(_keyJump);
        public bool GetThrow() => Input.GetKey(_keyThrow);

        public float GetMotionHorizontal()
        {
            var motion = Input.GetAxisRaw(_axisLeftStickX);
            Debug.Log(motion);
            return motion;         
        }
        public float GetMotionVertical()
        {
            return Input.GetAxisRaw(_axisLeftStickY);
        }

        public Vector2 GetAimDir()
        {
            throw new NotImplementedException("Aim axis support is not implimented.");
        }
    }
}