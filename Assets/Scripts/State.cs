using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T m_owner = null;

    public State(T owner)
    {
        m_owner = owner;
    }

    abstract public void Update();
    abstract public void FixedUpdate();
    abstract public void Enter(bool fixedUpdate);
    abstract public void Exit();
}
