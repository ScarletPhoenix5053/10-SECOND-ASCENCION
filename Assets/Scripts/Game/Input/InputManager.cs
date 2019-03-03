using System;
using UnityEngine;

namespace Sierra.AGPW.TenSecondAscencion
{
    public class InputManager : MonoBehaviour
    {
        private void Awake()
        {
            ControllerInput1 = new ControllerInputGroup(Player1ControllerIndex);
            ControllerInput2 = new ControllerInputGroup(Player2ControllerIndex);

            var keyboardInputAxis1 = new KeyboardInputAxis(Player1Right, Player1Left);
            var keyboardInputAxis2 = new KeyboardInputAxis(Player2Right, Player2Left);
            KeyboardInput1 = new KeyboardInputGroup(keyboardInputAxis1, Player1Jump, Player1Throw);
            KeyboardInput2 = new KeyboardInputGroup(keyboardInputAxis2, Player2Jump, Player2Throw);
        }
        private void Start()
        {
            LogControllerCountAndNames();
        }
        private void Update()
        {
            CheckControlsFor(Player1, ControllerInput1);
            CheckControlsFor(Player2, ControllerInput2);

            CheckControlsFor(Player1, KeyboardInput1);
            CheckControlsFor(Player2, KeyboardInput2);

            //LogAllKeyPresses()
        }

        public PlayerController Player1;
        public int Player1ControllerIndex = 1;
        public KeyCode Player1Left = KeyCode.A;
        public KeyCode Player1Right = KeyCode.D;
        public KeyCode Player1Jump = KeyCode.W;
        public KeyCode Player1Throw = KeyCode.F;

        public PlayerController Player2;
        public int Player2ControllerIndex = 2;
        public KeyCode Player2Left = KeyCode.LeftArrow;
        public KeyCode Player2Right = KeyCode.RightArrow;
        public KeyCode Player2Jump = KeyCode.UpArrow;
        public KeyCode Player2Throw = KeyCode.RightShift;

        private ControllerInputGroup ControllerInput1;
        private ControllerInputGroup ControllerInput2;
        private KeyboardInputGroup KeyboardInput1;
        private KeyboardInputGroup KeyboardInput2;

        private void LogControllerCountAndNames()
        {
            var padNames = Input.GetJoystickNames();

            Debug.Log("Found " + padNames.Length + " controllers");
            foreach (string pad in padNames)
            {
                Debug.Log(pad);
            }
        }
        private void LogAllKeyPresses()
        {
            Array values = Enum.GetValues(typeof(KeyCode));
            foreach (KeyCode code in values)
            {
                if (Input.GetKeyDown(code)) { print(Enum.GetName(typeof(KeyCode), code)); }
            }
        }
        private void CheckControlsFor(PlayerController player, IInputGroup inputGroup)
        {
            if (inputGroup.GetJump())
            {
                player.TriggerInputJump();
            }
            if (inputGroup.GetJumpHeld())
            {
                player.TriggerInputJumpHeld();
            }
            if (inputGroup.GetThrow())
            {
                player.TriggerInputThrow();
            }
            if (inputGroup.GetMotionHorizontal() <= -0.1 ||
                inputGroup.GetMotionHorizontal() >= 0.1)
            {
                player.TriggerInputHorizontal(
                    Mathf.Sign(
                        inputGroup.GetMotionHorizontal()
                        ));
            }
        }
    }
    public interface IInputGroup
    {
        bool GetJump();
        bool GetJumpHeld();
        bool GetThrow();

        float GetMotionHorizontal();
    }
    public struct KeyboardInputGroup : IInputGroup
    {
        public KeyboardInputGroup(KeyboardInputAxis horizontal, KeyCode keyJump, KeyCode keyThrow)
        {
            _keyThrow = keyThrow;
            _keyJump = keyJump;
            _axisHorizontalKeys = horizontal;
        }

        private readonly KeyboardInputAxis _axisHorizontalKeys;
        private readonly KeyCode _keyJump;
        private readonly KeyCode _keyThrow;

        public bool GetJump() => Input.GetKeyDown(_keyJump);
        public bool GetJumpHeld() => Input.GetKey(_keyJump);
        public bool GetThrow() => Input.GetKeyDown(_keyThrow);

        public float GetMotionHorizontal()
        {
            var motion = _axisHorizontalKeys.GetValue();
            return motion;
        }
    }
    public struct KeyboardInputAxis
    {
        public KeyboardInputAxis(KeyCode positive, KeyCode negative)
        {
            _positive = positive;
            _negative = negative;
        }

        private readonly KeyCode _positive;
        private readonly KeyCode _negative;

        public float GetValue()
        {
            var value = 0f;
            if (Input.GetKey(_positive)) value += 1;
            if (Input.GetKey(_negative)) value -= 1;
            return value;
        }
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

                _keyThrow = (KeyCode)Enum.Parse(typeof(KeyCode), "Joystick" + controllerIndex + "Button2");
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
        public bool GetThrow() => Input.GetKeyDown(_keyThrow);

        public float GetMotionHorizontal()
        {
            var motion = Input.GetAxisRaw(_axisLeftStickX);
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