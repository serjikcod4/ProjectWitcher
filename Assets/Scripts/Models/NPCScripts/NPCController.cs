﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(NPCPatrolController))]
[RequireComponent(typeof(NPCIdleController))]
[RequireComponent(typeof(NPCInteractController))]
[RequireComponent(typeof(RouteCompile))]
[RequireComponent(typeof(Mediator))]
[RequireComponent(typeof(NPCMove))]
[RequireComponent(typeof(CapsuleCollider))]
public class NPCController : MonoBehaviour
{
    bool onPatrol; //Находится ли нпс в патруле
    bool interact; // Взаимодействует ли с ним игрок
    bool onIdle; // Бездействует ли нпс
    Vector3 startPosition;
    float patrolRange;
    Vector3[] route; //
    int patrolChance = 5;
    int idleChance = 5;
    bool wait;
    GameObject player;

    private void Awake()
    {
        onPatrol = false;
        onIdle = false;
        interact = false;
        wait = false;
        startPosition = transform.position;
        patrolRange = 15;
        Mediator.InteractEvent += InteractPlayer;
        NPCPatrolController.PatrolEvent += PatrolWaiter;
        NPCIdleController.IdleEvent += IdleWaiter;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (wait)
        {
            DisableAll();
            GetComponent<NPCPatrolController>().Stop();
            Debug.Log("Waiting Player");
            if (interact)
            {

            }
        }
        else if(onPatrol)
        {
            GetComponent<NPCPatrolController>().Patrol(route);
        }
        else if(onIdle)
        {
            
        }
        else
        {
            int choseAct = Random.Range(-patrolChance, idleChance);
            if(choseAct < 0)
            {
                onPatrol = true;
                route = GetComponent<RouteCompile>().Compile(startPosition, patrolRange);
                patrolChance--;
                idleChance = 5;
            }
            else
            {
                onIdle = true;
                GetComponent<NPCIdleController>().Idle();
                idleChance--;
                patrolChance = 5;
            }
        }
    }

    private void DisableAll()
    {
        onIdle = false;
        onPatrol = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            wait = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            wait = false;
        }
    }

    /// <summary>
    /// Методы подписывающиеся на события для отслеживания состояний
    /// </summary>
    /// <param name="condition"></param>
    #region Subscribers
    private void InteractPlayer(bool condition)
    {
        interact = condition;
    }
    private void PatrolWaiter()
    {
        onPatrol = false;
    }
    private void IdleWaiter()
    {
        onIdle = false;
    }
    #endregion

}
