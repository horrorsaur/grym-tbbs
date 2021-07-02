// GENERATED AUTOMATICALLY FROM 'Assets/_Scripts/Input System/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Battle"",
            ""id"": ""7aafdbcc-e917-4818-b494-025234cb1b76"",
            ""actions"": [
                {
                    ""name"": ""NavigateBattleUI"",
                    ""type"": ""Button"",
                    ""id"": ""84ecbfc7-1d2b-441b-b7a1-fe7f5b9b8957"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""7765d7c2-1b6c-49df-9ebf-faf72337d481"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Back"",
                    ""type"": ""Button"",
                    ""id"": ""e69c7c83-4eb6-44e0-aa09-76445b35f4d0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ArrowKeys"",
                    ""id"": ""0445f897-31e0-4867-8cb1-48617eafd86d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NavigateBattleUI"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""24e22748-16be-44c4-8487-8081d933392c"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""NavigateBattleUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d4fbb26a-c4ea-41a5-b672-c0b810aa7aad"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""NavigateBattleUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d0a2576e-3f37-4891-af64-65b8de707b80"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""NavigateBattleUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""09ad3519-46af-4833-b819-ffb6f952290a"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""NavigateBattleUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""5867803b-2450-4886-873a-c4bcbfdc26ca"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""988223cd-e432-4ff3-b8dc-1c8dba2ec14f"",
                    ""path"": ""<SwitchProControllerHID>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Nintendo Controls"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3f69e23-4db6-4bbb-8c04-e5c24e1a500f"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC Controls"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ccd893a9-6902-4149-90eb-174848df7cb3"",
                    ""path"": ""<SwitchProControllerHID>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Nintendo Controls"",
                    ""action"": ""Back"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Nintendo Controls"",
            ""bindingGroup"": ""Nintendo Controls"",
            ""devices"": [
                {
                    ""devicePath"": ""<SwitchProControllerHID>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PC Controls"",
            ""bindingGroup"": ""PC Controls"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Battle
        m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
        m_Battle_NavigateBattleUI = m_Battle.FindAction("NavigateBattleUI", throwIfNotFound: true);
        m_Battle_Submit = m_Battle.FindAction("Submit", throwIfNotFound: true);
        m_Battle_Back = m_Battle.FindAction("Back", throwIfNotFound: true);
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

    // Battle
    private readonly InputActionMap m_Battle;
    private IBattleActions m_BattleActionsCallbackInterface;
    private readonly InputAction m_Battle_NavigateBattleUI;
    private readonly InputAction m_Battle_Submit;
    private readonly InputAction m_Battle_Back;
    public struct BattleActions
    {
        private @PlayerInput m_Wrapper;
        public BattleActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @NavigateBattleUI => m_Wrapper.m_Battle_NavigateBattleUI;
        public InputAction @Submit => m_Wrapper.m_Battle_Submit;
        public InputAction @Back => m_Wrapper.m_Battle_Back;
        public InputActionMap Get() { return m_Wrapper.m_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActions instance)
        {
            if (m_Wrapper.m_BattleActionsCallbackInterface != null)
            {
                @NavigateBattleUI.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnNavigateBattleUI;
                @NavigateBattleUI.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnNavigateBattleUI;
                @NavigateBattleUI.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnNavigateBattleUI;
                @Submit.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnSubmit;
                @Back.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnBack;
                @Back.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnBack;
                @Back.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnBack;
            }
            m_Wrapper.m_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @NavigateBattleUI.started += instance.OnNavigateBattleUI;
                @NavigateBattleUI.performed += instance.OnNavigateBattleUI;
                @NavigateBattleUI.canceled += instance.OnNavigateBattleUI;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Back.started += instance.OnBack;
                @Back.performed += instance.OnBack;
                @Back.canceled += instance.OnBack;
            }
        }
    }
    public BattleActions @Battle => new BattleActions(this);
    private int m_NintendoControlsSchemeIndex = -1;
    public InputControlScheme NintendoControlsScheme
    {
        get
        {
            if (m_NintendoControlsSchemeIndex == -1) m_NintendoControlsSchemeIndex = asset.FindControlSchemeIndex("Nintendo Controls");
            return asset.controlSchemes[m_NintendoControlsSchemeIndex];
        }
    }
    private int m_PCControlsSchemeIndex = -1;
    public InputControlScheme PCControlsScheme
    {
        get
        {
            if (m_PCControlsSchemeIndex == -1) m_PCControlsSchemeIndex = asset.FindControlSchemeIndex("PC Controls");
            return asset.controlSchemes[m_PCControlsSchemeIndex];
        }
    }
    public interface IBattleActions
    {
        void OnNavigateBattleUI(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnBack(InputAction.CallbackContext context);
    }
}
