using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentPath : MonoBehaviour {

    public float m_Width = 0.5f;
    NavMeshAgent m_Agent;
    LineRenderer m_Line;

    // Use this for initialization
    void Start () {
        m_Agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        if(m_Agent == null)
        {
            return;
        }

        m_Line = this.GetComponent<LineRenderer>();
        if (m_Line == null)
        {
            m_Line = this.gameObject.AddComponent<LineRenderer>();
            m_Line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            m_Line.startWidth = m_Width;
            m_Line.endWidth = m_Width;
            m_Line.startColor = Color.yellow;
            m_Line.endColor = Color.yellow;
        }
        
        if(m_Agent.path.corners.Length < 2)
        {
            m_Line.enabled = false;
            return;
        }
        else
        {
            m_Line.enabled = true;
        }

        m_Line.positionCount = m_Agent.path.corners.Length;
        for (int i = 0; i < m_Agent.path.corners.Length; i++)
        {
            m_Line.SetPosition(i, m_Agent.path.corners[i]);
        }
    }
}
