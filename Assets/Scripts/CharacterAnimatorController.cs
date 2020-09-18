﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent = null;
    private Animator animatorController;
    private void Awake()
    {
        animatorController = GetComponent<Animator>();
    }
    void Update()
    {
        float speed = navMeshAgent.velocity.magnitude;
        if (speed < 0.1f) speed = 0;
        animatorController.SetFloat("Speed", speed);
    }
}
