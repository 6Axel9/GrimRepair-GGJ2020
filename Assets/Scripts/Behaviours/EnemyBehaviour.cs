using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyBehaviour : MonoBehaviour
{
    private enum StatusFlags
    {
        BottomFloor = 00000000,
        TopFloor    = 00000001,
        OnLedge     = 00000010,
    }

    [SerializeField]
    private SpawnerBehaviour m_spawner;
    [SerializeField]
    private NavMeshAgent m_agent;
    [SerializeField]
    private Rigidbody m_rigidbody;
    [SerializeField]
    [Range(100f, 1000f)]
    private float m_moveSpeed = 500f;

    private List<InteractableBehaviour> m_surroundings;
    private StatusFlags m_status;

    private void Start()
    {
        m_surroundings = new List<InteractableBehaviour>();
    }

    private void Update()
    {
        if (!m_agent.hasPath)
            m_agent.SetDestination(m_spawner.FindClosest(InteractableType.Collectable, transform.position).position);

        if (m_surroundings.Count == 0)
            return;
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableBehaviour interactable = other.GetComponent<InteractableBehaviour>();
        if (interactable == null)
            return;

        if(interactable.Type == InteractableType.Collectable)
        {
            Destroy(interactable.gameObject);
            m_spawner.Respawn(interactable.Type);
            Debug.Log("Collected");
            return;
        }

        if (!m_surroundings.Contains(interactable))
            m_surroundings.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableBehaviour interactable = other.GetComponent<InteractableBehaviour>();
        if (interactable == null || interactable.Type == InteractableType.Collectable)
            return;

        if (m_surroundings.Contains(interactable))
            m_surroundings.Remove(interactable);
    }

    private void Interact(InteractableType interactableType)
    {
        switch (interactableType)
        {
            case InteractableType.Ledge:
                LedgeAction();
                break;
            case InteractableType.Stairs:
                StairsAction();
                break;
            case InteractableType.Trigger:
                Debug.Log("Triggered");
                break;
        }
    }

    private void LedgeAction()
    {
        if (!m_status.HasFlag(StatusFlags.OnLedge))
        {
            m_status |= StatusFlags.OnLedge;
            Debug.Log("Jumped");
        }
        else
        {
            m_status &= ~StatusFlags.OnLedge;
            Debug.Log("Dropped");
        }
    }

    private void StairsAction()
    {
        if (m_status.HasFlag(StatusFlags.OnLedge))
            return;

        if (!m_status.HasFlag(StatusFlags.TopFloor))
        {
            m_status |= StatusFlags.TopFloor;
            Debug.Log("Upstairs");
        }
        else
        {
            m_status &= ~StatusFlags.TopFloor;
            Debug.Log("Downstairs");
        }
    }

}
