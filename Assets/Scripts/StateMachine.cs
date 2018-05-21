using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : MonoBehaviour
{
    private struct StateInfo
    {
        public StateInfo(State<T> state, string name)
        {
            this.state = state;
            this.name = name;
        }
        public State<T> state;
        public string name;
    }
    private List<StateInfo> m_knownStates = null;
    private Queue<State<T>> m_states = null;

    public StateMachine()
    {
        m_states = new Queue<State<T>>();
        m_knownStates = new List<StateInfo>();
    }

    public void AddState(State<T> state, string name)
    {
        m_knownStates.Add(new StateInfo(state, name));
    }

    public void PushState(string state)
    {
        for (int i = 0; i < m_knownStates.Count; ++i)
        {
            if (m_knownStates[i].name == state)
            {
                m_states.Enqueue(m_knownStates[i].state);
                break;
            }
        }
    }

    public void PopStates()
    {
        m_states.Dequeue();
    }

    public void ClearStates()
    {
        m_states.Clear();
    }

    public void Update()
    {
        State<T> state = m_states.Peek();
        state.Enter(false);
        state.Update();
        state.Exit();
    }

    public void FixedUpdate()
    {
        State<T> state = m_states.Peek();
        state.Enter(true);
        state.FixedUpdate();
        state.Exit();
    }

    public void LateUpdate()
    {
        //PopStates();
    }
}
