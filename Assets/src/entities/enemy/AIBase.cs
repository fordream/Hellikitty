using System;
using UnityEngine;

public enum GeneralAIState
{
    NONE, 
    WALKING, 
    SHOOTING
};

public class AIBase : MonoBehaviour
{
    [HideInInspector] public GeneralAIState general_ai_state = GeneralAIState.NONE;
}
