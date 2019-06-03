using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Controllers
{
    public class EnemyAttackController : MonoBehaviour
    {
        
        private bool IsHit;
        private float damage = 10;
        

        

        private void OnTriggerEnter(Collider collider)
        {
            Debug.Log($"{collider} entered");
            IDamageable target = collider.GetComponent<IDamageable>();
            if (target != null)
            {
                Debug.Log($"Attack => {collider}");
                IsHit = true;
                target.TakeDamage(damage);
            }
            else
            {
                IsHit = false;
            }
        }



        //public override void ControllerUpdate()
        //{

        //}


    }
}
