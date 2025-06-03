using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class KeyBindSettings
{
    [Serializable]
    public class KeyBindPair
    {
        public InputAction action;
        public KeyCode keyCode;
    }

    public List<KeyBindPair> keyBinds = new List<KeyBindPair>();
    public List<KeyBindPair> defaultKeyBinds;

    public KeyBindSettings()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        keyBinds = new List<KeyBindPair>
        {
            new KeyBindPair { action = InputAction.Attack, keyCode = KeyCode.Mouse0 },
            new KeyBindPair { action = InputAction.Parry, keyCode = KeyCode.Mouse1 },
            new KeyBindPair { action = InputAction.MoveForward, keyCode = KeyCode.W },
            new KeyBindPair { action = InputAction.MoveLeft, keyCode = KeyCode.A },
            new KeyBindPair { action = InputAction.MoveBackward, keyCode = KeyCode.S },
            new KeyBindPair { action = InputAction.MoveRight, keyCode = KeyCode.D },
            new KeyBindPair { action = InputAction.Sprint, keyCode = KeyCode.LeftShift },
            new KeyBindPair { action = InputAction.Roll, keyCode = KeyCode.Space },
            new KeyBindPair { action = InputAction.Use, keyCode = KeyCode.E},
            new KeyBindPair { action = InputAction.UseItem, keyCode = KeyCode.B },
            new KeyBindPair { action = InputAction.SwitchWeapon, keyCode = KeyCode.Q },
            new KeyBindPair { action = InputAction.OpenInventory, keyCode = KeyCode.Tab },
            new KeyBindPair { action = InputAction.OpenRebindPanel, keyCode = KeyCode.Escape },
            new KeyBindPair { action = InputAction.ShowAbilityInformation, keyCode = KeyCode.G },
            new KeyBindPair { action = InputAction.AbilButton1, keyCode = KeyCode.Z },
            new KeyBindPair { action = InputAction.AbilButton2, keyCode = KeyCode.X },
            new KeyBindPair { action = InputAction.AbilButton3, keyCode = KeyCode.C },
            new KeyBindPair { action = InputAction.AbilButton4, keyCode = KeyCode.V },
            new KeyBindPair { action = InputAction.ConfirmAbility, keyCode = KeyCode.R },
         };
         defaultKeyBinds = keyBinds;
    }
}