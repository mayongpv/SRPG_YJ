using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public Player SelectPlayer;
    Animator animator;
    private void Start()
    {
        SelectPlayer = this;
        animator = GetComponentInChildren<Animator>();
    }
    public void PlayAnimation(string nodName)
    {
        animator.Play(nodName, 0, 0);
    }
}
