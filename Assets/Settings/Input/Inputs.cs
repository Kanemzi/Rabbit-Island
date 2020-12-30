// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Hand"",
            ""id"": ""c68418b9-f3f3-4baa-8881-604bb4087f33"",
            ""actions"": [
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""98d5831f-5ea3-497f-97af-7bfd2ca7256e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""75658736-901e-4696-b951-9f94f37d62ca"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard-Mouse"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5be4c1f2-f85c-4e68-83c8-be001856f6fc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard-Mouse"",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Endmenu"",
            ""id"": ""0fd9c247-3ae2-4e3f-9e79-f4a2d0a208f4"",
            ""actions"": [
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""fa4def93-f446-4868-8dd6-28b577d15d53"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7a642f35-b263-488d-b5a6-ca23add22e10"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard-Mouse"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard-Mouse"",
            ""bindingGroup"": ""Keyboard-Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Hand
        m_Hand = asset.FindActionMap("Hand", throwIfNotFound: true);
        m_Hand_Grab = m_Hand.FindAction("Grab", throwIfNotFound: true);
        // Endmenu
        m_Endmenu = asset.FindActionMap("Endmenu", throwIfNotFound: true);
        m_Endmenu_Restart = m_Endmenu.FindAction("Restart", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Hand
    private readonly InputActionMap m_Hand;
    private IHandActions m_HandActionsCallbackInterface;
    private readonly InputAction m_Hand_Grab;
    public struct HandActions
    {
        private @Inputs m_Wrapper;
        public HandActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Grab => m_Wrapper.m_Hand_Grab;
        public InputActionMap Get() { return m_Wrapper.m_Hand; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HandActions set) { return set.Get(); }
        public void SetCallbacks(IHandActions instance)
        {
            if (m_Wrapper.m_HandActionsCallbackInterface != null)
            {
                @Grab.started -= m_Wrapper.m_HandActionsCallbackInterface.OnGrab;
                @Grab.performed -= m_Wrapper.m_HandActionsCallbackInterface.OnGrab;
                @Grab.canceled -= m_Wrapper.m_HandActionsCallbackInterface.OnGrab;
            }
            m_Wrapper.m_HandActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Grab.started += instance.OnGrab;
                @Grab.performed += instance.OnGrab;
                @Grab.canceled += instance.OnGrab;
            }
        }
    }
    public HandActions @Hand => new HandActions(this);

    // Endmenu
    private readonly InputActionMap m_Endmenu;
    private IEndmenuActions m_EndmenuActionsCallbackInterface;
    private readonly InputAction m_Endmenu_Restart;
    public struct EndmenuActions
    {
        private @Inputs m_Wrapper;
        public EndmenuActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Restart => m_Wrapper.m_Endmenu_Restart;
        public InputActionMap Get() { return m_Wrapper.m_Endmenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EndmenuActions set) { return set.Get(); }
        public void SetCallbacks(IEndmenuActions instance)
        {
            if (m_Wrapper.m_EndmenuActionsCallbackInterface != null)
            {
                @Restart.started -= m_Wrapper.m_EndmenuActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_EndmenuActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_EndmenuActionsCallbackInterface.OnRestart;
            }
            m_Wrapper.m_EndmenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
            }
        }
    }
    public EndmenuActions @Endmenu => new EndmenuActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard-Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IHandActions
    {
        void OnGrab(InputAction.CallbackContext context);
    }
    public interface IEndmenuActions
    {
        void OnRestart(InputAction.CallbackContext context);
    }
}
