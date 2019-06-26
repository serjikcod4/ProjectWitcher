using Assets.Scripts.Models.ConditionsAndActions.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemySpace
{
    public enum Speed
    {
        Walk,
        Run,
        Throw
    }

    public class EnemyMove
    {
        private float speed;
        private NavMeshAgent agent;
        private Rigidbody rb;
        public BaseCharacterModel CharacterModel;

        public EnemyMove(NavMeshAgent agent, BaseCharacterModel CharacterModel, Rigidbody rb)
        {
            this.agent = agent;
            this.CharacterModel = CharacterModel;
            this.rb = rb;
        }

        public void Move(Vector3 direction)
        {
            agent.speed = CharacterModel.Speed;
            agent.SetDestination(direction);
        }

        public void Move(Vector3 direction, Speed Speed)
        {
            switch(Speed)
            {
                case Speed.Walk:
                    agent.speed = CharacterModel.Speed;
                    break;

                case Speed.Run:
                    agent.speed = CharacterModel.RunSpeed;
                    break;

                case Speed.Throw:
                    agent.speed = CharacterModel.RunSpeed * 2;
                    break;
            }
            
            agent.SetDestination(direction);
        }

        public void Rotate(Vector3 direction)
        {
            direction.y = 0f;
            direction = direction.normalized;
            Quaternion newRotation = Quaternion.LookRotation(direction);
            rb.MoveRotation(newRotation);
        }
        public void Stop()
        {
            agent.isStopped = true;
        }
        public void Continue()
        {
            agent.isStopped = false;
        }
    }
}
