using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType { Ledge, Stairs, Trigger, Collectable }

public class InteractableBehaviour : MonoBehaviour
{
    [SerializeField]
    private InteractableType m_type;
    public InteractableType Type => m_type;
}
