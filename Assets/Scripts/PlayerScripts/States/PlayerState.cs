using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerMovement player;
    protected Rigidbody2D rb;
    protected PlayerStats stats;
    protected Animator animator;

    public PlayerState(PlayerMovement player, Rigidbody2D rb, PlayerStats stats, Animator animator)
    {
        this.player = player;
        this.rb = rb;
        this.stats = stats;
        this.animator = animator;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}
    public virtual void Update() {}
    public virtual void FixedUpdate() {}
}
