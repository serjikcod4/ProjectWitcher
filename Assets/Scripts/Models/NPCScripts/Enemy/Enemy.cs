﻿using Assets.Scripts.Interfaces;
using Assets.Scripts.Models.ConditionsAndActions;
using Assets.Scripts.Models.ConditionsAndActions.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySpace
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(SphereCollider))]
    public class Enemy : MonoBehaviour, ISetDamage, IGetConditions
    {
        public delegate void SeeEnemy(string unitName);
        public static event SeeEnemy SeeEvent;
        public delegate void TakeDamage(float dmg, string unitName);
        public static event TakeDamage DamageEvent;
        

        EnemyController controller;
        [SerializeField] EnemySpecifications specification;
        Transform _transform;
        Transform head;
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        AudioSource gunShotSound;
        MeshRenderer mesh;
        MeshRenderer headMesh;
        NavMeshAgent agent;
        Rigidbody rb;
        CapsuleCollider enemyBorder;
        SphereCollider enemyView;
        LineRenderer shootLine;
        GameObject player;
        
        BaseConditions conditions;

        public void EnemyAwake()
        {
            _transform = GetComponent<Transform>();
            head = _transform.GetChild(0);
            mesh = GetComponent<MeshRenderer>();
            headMesh = head.GetComponent<MeshRenderer>();
            gun = _transform.GetChild(1).GetComponent<MeshRenderer>();
            knife = _transform.GetChild(3).GetComponent<MeshRenderer>();
            knife.enabled = false;
            gunBarrelEnd = _transform.GetChild(2);
            gunShotSound = gunBarrelEnd.GetComponent<AudioSource>();
            headMesh.material.color = new Color(1, 0.9058824f, 0.6745098f, 1);
            agent = GetComponent<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
            enemyBorder = GetComponent<CapsuleCollider>();
            enemyView = GetComponent<SphereCollider>();
            shootLine = GetComponentInChildren<LineRenderer>();
            player = GameObject.FindGameObjectWithTag("Player");
            enemyView.radius = specification.ViewDistance;

            conditions = new BaseConditions(specification.GetCharacterConditionsList(), new CharacterConditionsSpecifications(specification));

            controller = new EnemyController(_transform, agent, mesh, headMesh, gun, knife, gunBarrelEnd, rb, enemyBorder,
                enemyView, shootLine, specification, _transform.position, player, gunShotSound, conditions);
            controller.EnemyControllerAwake();
        }

        public void EnemyUpdate(float deltaTime)
        {
            controller.EnemyControllerUpdate(deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject == player)
            {
                SeeEvent(_transform.name);
            }
        }

        public void ApplyDamage(float damage)
        {
            DamageEvent(damage, _transform.name);
        }

        /// <summary>
        /// Метод применения состояний к персонажу
        /// </summary>
        /// <param name="Characteristics">Массив состояний</param>
        public void ApplyCondition(params CurrentCondition[] Characteristics)
        {
            foreach (var Condition in Characteristics)
            {
                if (conditions.Conditions.HasName(Condition.Name))
                {
                    ConditionChance(Condition);
                }
            }
        }

        /// <summary>
        /// Рассчет получения конкретного статуса
        /// </summary>
        /// <param name="Condition"></param>
        private void ConditionChance(CurrentCondition Condition)
        {
            //НЕТ РОЛЕВОЙ СИСТЕМЫ!!!

            conditions.Conditions.ChangeConditionStatus(Condition.Name, true);
        }
    }
}

