using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAgentSync : MonoBehaviour 
{
    MyAgent agent;
    Animator animator;

    void Update() {
        if (agent?.agent.angularSpeed != 0) {
            if (agent.OnAgentJump()) animator.Play("jump");
            else if (agent.OnAgentHighIdle()) animator.Play("sit");
            else if (agent.OnAgentIdle()) animator.Play("idle");
            else if (agent.OnAgentMove()) animator.Play("walk");
            
        }
    }

    public bool OnDestinationReach() {
        return agent.OnAgentReachDestination();
    }

    public void LinkAnimatorToAgent(MyAgent agent) {
        this.agent = agent;
        animator = agent.agent.transform.GetComponent<Animator>();
    }
}