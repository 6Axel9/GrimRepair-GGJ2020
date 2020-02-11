using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
using TMPro;

public enum PlayerType { Auto, Manual }

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField]
    private SpawnerBehaviour m_spawner;
    [SerializeField]
    private NavMeshAgent m_agent;
    public NavMeshAgent Agent => m_agent;
    [SerializeField]
    private GameObject m_wrapper;
    [SerializeField]
    private PlayerMapping m_mapping;
    public PlayerMapping Mapping => m_mapping;

    [SerializeField]
    private TMP_Text m_scoreText;
    private float m_score;
    public float Score
    {
        get => m_score;
        set
        {
            m_score = value;
            m_scoreText.text = m_score.ToString();
        }
    }

    private void Start()
    {
        m_wrapper.LeanMoveLocalY(0.1f, 0.5f).setLoopPingPong().setEaseInOutSine();
    }

    private void Update()
    {
        if (!m_agent.enabled)
            return;

        if (m_mapping.Controls == PlayerType.Auto)
        {
            if (!m_agent.hasPath)
                m_agent.SetDestination(m_spawner.FindClosest(m_mapping.Collectable, transform.position).position);
            if (m_agent.velocity.sqrMagnitude > 0.1f)
                m_wrapper.transform.localEulerAngles = Vector3.LerpUnclamped(m_wrapper.transform.localEulerAngles, new Vector3(30f,0f,0f), Time.deltaTime * 10f);
            else
                m_wrapper.transform.localEulerAngles = Vector3.LerpUnclamped(m_wrapper.transform.localEulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);

        }
        else if(m_mapping.Controls == PlayerType.Manual)
        {
            if (!Input.anyKey)
            {
                m_wrapper.transform.localEulerAngles = Vector3.LerpUnclamped(m_wrapper.transform.localEulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
                return;
            }

            Vector3 direction = Vector3.zero;

            if (Input.GetKey(m_mapping.Left))
                direction.x--;
            if (Input.GetKey(m_mapping.Right))
                direction.x++;
            if (Input.GetKey(m_mapping.Up))
                direction.z++;
            if (Input.GetKey(m_mapping.Down))
                direction.z--;

            m_agent.Move(direction * m_mapping.Step * Time.deltaTime);
            transform.forward = Vector3.SlerpUnclamped(transform.forward, direction, 10f * Time.deltaTime);
            if (direction.sqrMagnitude > 0.1f)
                m_wrapper.transform.localEulerAngles = Vector3.LerpUnclamped(m_wrapper.transform.localEulerAngles, new Vector3(30f, 0f, 0f), Time.deltaTime * 10f);
            else
                m_wrapper.transform.localEulerAngles = Vector3.LerpUnclamped(m_wrapper.transform.localEulerAngles, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
                
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       CollectableBehaviour collectable =  other.GetComponent<CollectableBehaviour>();
        if (collectable == null || collectable.Type != m_mapping.Collectable)
            return;

        m_spawner.Respawn(collectable.Type);
        Destroy(collectable.gameObject);
        Score = m_score + 1;
    }
}

[Serializable]
public class PlayerMapping
{
    [SerializeField]
    private Sprite m_winnerIco;
    public Sprite WinnerIco => m_winnerIco;
    [SerializeField]
    private CollectableType m_collectable;
    public CollectableType Collectable => m_collectable;
    [SerializeField]
    private PlayerType m_controls;
    public PlayerType Controls { get => m_controls; set => m_controls = value; }
    [SerializeField]
    private KeyCode m_left;
    public KeyCode Left => m_left;
    [SerializeField]
    private KeyCode m_right;
    public KeyCode Right => m_right;
    [SerializeField]
    private KeyCode m_up;
    public KeyCode Up => m_up;
    [SerializeField]
    private KeyCode m_down;
    public KeyCode Down => m_down;
    [SerializeField]
    [Range(1f, 100f)]
    private float m_step = 10;
    public float Step => m_step;
}
