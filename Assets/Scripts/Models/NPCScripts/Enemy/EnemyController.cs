using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Assets.Scripts.Models.ConditionsAndActions;
using Assets.Scripts.Models.ConditionsAndActions.Helpers;

namespace EnemySpace
{
    public class EnemyController
    {
        #region OldFields

        //private string type;
        //private float hp;
        //public float CurrentHP { get; private set; }
        //private float speed;
        //private float runSpeed;
        //float patrolDistance; //радиус зоны патрулирования
        //float chasingTime;
        //float rangeDistance;
        //float rangeDamage;
        //float rangeAccuracy;
        //float shootSpeed;
        //float meleeDistance;
        //float meleeDamage;
        //float hitSpeed;

        private BaseCharacterModel CharacterModel;

        private readonly float MaximumHP;

        Vector3 homePoint; //стартовая позиция для генерации зоны патрулирования

        float timer = 0f;

        #endregion

        public delegate void HealthCheck();
        public event HealthCheck CheckHealth;

        #region Specification

        EnemySpecifications spec;

        #endregion
        
        #region Actions
        bool alive; //жив ли враг
        bool onPatrol; //Режим патрулирования
        bool inChase; // Режим погони
        bool onIdle; // Режим бездействия
        bool inFight; //Режим сражения
        bool comingHome;
        #endregion

        Vector3[] route; //маршрут точек для патруля   

        /// <summary>
        /// Шансы выбора действия
        /// </summary>
        #region Chances
        int patrolChance = 5;//шанс выбора режима патрулирования
        int idleChance = 5;//шанс выбора режима бездействия
        #endregion

        /// <summary>
        /// Дополнительно подключаемые компоненты, не наследованные от монобеха
        /// </summary>
        #region Instance
        RouteCompile RouteGenerator;
        EnemyDie Dying;
        EnemyMove Move;
        EnemyChase Chasing;
        EnemyPatrolController Patroling;
        EnemyIdleController Idle;
        EnemyComingHome ComingHome;
        EnemyFightController Fight;
        EnemyHurt Hurt;
        #endregion

        /// <summary>
        /// Кэшированные компоненты
        /// </summary>
        #region Cache
        MeshRenderer mesh;
        MeshRenderer headMesh;
        Transform enemyTransform;
        MeshRenderer gun;
        MeshRenderer knife;
        Transform gunBarrelEnd;
        NavMeshAgent agent;
        Rigidbody rb;
        CapsuleCollider enemyBorder;
        SphereCollider enemyView;
        LineRenderer shootLine;
        AudioSource gunShotSound;
        GameObject player;//игрок
        #endregion

        //NEW
        #region Состояния

        BaseConditions conditions;

        #endregion

        public EnemyController(Transform enemyTransform, NavMeshAgent agent, MeshRenderer mesh, MeshRenderer headMesh, 
            MeshRenderer gun, MeshRenderer knife, Transform gunBarrelEnd, Rigidbody rb, CapsuleCollider enemyBorder, 
            SphereCollider enemyView, LineRenderer shootLine, EnemySpecifications spec, Vector3 homePoint, 
            GameObject player, AudioSource gunShotSound, BaseConditions conditions)
        {
            this.enemyTransform = enemyTransform;
            this.agent = agent;
            this.mesh = mesh;
            this.headMesh = headMesh;
            this.enemyBorder = enemyBorder;
            this.enemyView = enemyView;
            this.rb = rb;
            this.shootLine = shootLine;
            this.gun = gun;
            this.knife = knife;
            this.gunBarrelEnd = gunBarrelEnd;
            this.gunShotSound = gunShotSound;
            this.homePoint = homePoint;
            this.player = player;

            #region Mod

            #region Old

            //this.hp = spec.HP;
            //this.speed = spec.Speed;
            //this.runSpeed = spec.RunSpeed;
            //this.chasingTime = spec.ChasingTime;
            //this.patrolDistance = spec.PatrolDistance;
            //this.type = spec.Type;
            //this.rangeDistance = spec.RangeDistance;
            //this.rangeDamage = spec.RangeDamage;
            //this.rangeAccuracy = spec.RangeAccuracy;
            //this.shootSpeed = spec.ShootSpeed;
            //this.meleeDistance = spec.MeleeDistance;
            //this.meleeDamage = spec.MeleeDamage;
            //this.hitSpeed = spec.HitSpeed;

            #endregion

            this.spec = spec;

            MaximumHP = spec.HP;

            this.conditions = conditions;

            CharacterModel = new BaseCharacterModel(spec.HP, spec.Speed, spec.RunSpeed);

            #endregion

        }

        public void EnemyControllerAwake()
        {
            alive = true;
            onPatrol = false;
            onIdle = false;
            inChase = false;
            inFight = false;
            comingHome = false;

            RouteGenerator = new RouteCompile();
            Dying = new EnemyDie(enemyTransform);
            Move = new EnemyMove(agent, CharacterModel, rb);
            if(spec.Type == "Range")
            {
                Chasing = new EnemyChase(Move, enemyTransform, spec.ChasingTime, spec.RangeDistance);
            }
            else if(spec.Type == "Melee")
            {
                Chasing = new EnemyChase(Move, enemyTransform, spec.ChasingTime, spec.MeleeDistance);
            }           
            Patroling = new EnemyPatrolController(Move, enemyTransform);
            Idle = new EnemyIdleController(enemyTransform);
            ComingHome = new EnemyComingHome(Move, enemyTransform, homePoint);
            if(spec.Type == "Range")
            {
                Fight = new EnemyFightController(Move, enemyTransform, gun, knife, gunBarrelEnd, shootLine, spec.RangeDistance, spec.MeleeDistance, spec.RangeDamage, spec.RangeAccuracy, spec.ShootSpeed, spec.MeleeDamage, spec.HitSpeed,  gunShotSound);
            }
            if (spec.Type == "Melee")
            {
                Fight = new EnemyFightController(Move, enemyTransform, gun, knife, gunBarrelEnd, shootLine, spec.RangeDistance, spec.MeleeDistance, spec.RangeDamage, spec.RangeAccuracy, spec.ShootSpeed, spec.MeleeDamage, spec.HitSpeed, gunShotSound);
            }
            Hurt = new EnemyHurt(headMesh);

            ///<summary>
            ///подписка на события
            /// </summary>
            EnemyPatrolController.PatrolEvent += PatrolWaiter;
            EnemyIdleController.IdleEvent += IdleWaiter;
            EnemyChase.ChaseEvent += FinishChase;
            EnemyChase.AttackSwitchEvent += AttackModeOn;
            Enemy.SeeEvent += StartChase;
            Enemy.DamageEvent += TakeDamage;
            EnemyComingHome.ComingHomeEvent += AtHome;
            EnemyFightController.AttackToChaseEvent += AttackModeOff;
            CheckHealth += CharacterHealthCheck;

        }

        public void EnemyControllerUpdate(float deltaTime)
        {
            if(alive)
            {
                if (conditions.HasActiveConditions)
                {
                    CheckHealth.Invoke();
                    conditions.ConditionsUpdateStart(ref Move.CharacterModel, ref spec, deltaTime);
                }
                
                ///<summary>
                ///Проверка состояния деятельности врага
                /// </summary>
                if(inChase)
                {
                    enemyView.enabled = false;
                    Chasing.Chase(player, deltaTime);
                    //Debug.Log("Chasing");
                }
                else if (comingHome)
                {
                    //Debug.Log("ComingHome");
                    timer += deltaTime;
                    ComingHome.ComingHome();
                    if(timer > 3f && !enemyView.enabled)
                    {
                        //Debug.Log("ViewEnabled");
                        enemyView.enabled = true;
                    }
                }
                else if (onPatrol)
                {
                    Patroling.Patrol(route);
                }
                else if (onIdle)
                {
                    Idle.Idle();
                }
                else if (inFight)
                {
                    Fight.Fight(player, deltaTime);
                }
                else
                {
                    int choseAct = Random.Range(-patrolChance, idleChance);
                    if (choseAct < 0)
                    {
                        onPatrol = true;
                        route = RouteGenerator.Compile(homePoint, spec.PatrolDistance);
                        patrolChance--;
                        idleChance = 5;
                    }
                    else
                    {
                        onIdle = true;
                        idleChance--;
                        patrolChance = 5;
                    }
                }
            }
            else
            {
                Move.Stop();
                headMesh.enabled = false;
                gun.enabled = false;
                knife.enabled = false;
                enemyBorder.enabled = false;//выключаем коллайдер
                rb.constraints = RigidbodyConstraints.FreezeAll;//замораживаем перемещения и повороты
                Dying.Die(mesh, deltaTime);//запускаем событие смерти
            }
        }

        /// <summary>
        /// Методы подписывающиеся на события для отслеживания состояний
        /// </summary>
        /// <param name="condition"></param>
        #region Subscribers
        private void FinishChase(string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                inChase = false;
                comingHome = true;
                timer = 0f;
            }
        }
        private void AtHome(string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                comingHome = false;
            }
        }
        private void AttackModeOn(string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                inFight = true;
                inChase = false;
            }
        }
        private void AttackModeOff(string unitName)
        {
            if (enemyTransform.name == unitName)
            {
                inFight = false;
                inChase = true;
            }
        }
        private void PatrolWaiter(string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                onPatrol = false;
            }           
        }
        private void IdleWaiter(string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                onIdle = false;
            }
        }
        private void StartChase(string unitName)
        {
            if(unitName == enemyTransform.name)
            {
                inChase = true;
                onIdle = false;
                onPatrol = false;
            }
        }
        private void TakeDamage(float dmg, string unitName)
        {
            if(enemyTransform.name == unitName)
            {
                if (CharacterModel.Health > dmg & CharacterModel.Health > 0)
                {
                    CharacterModel.Health -= dmg;
                    float lifePercent = CharacterModel.Health / MaximumHP * 100;
                    Hurt.Hurt(lifePercent);
                }
                else
                {
                    CharacterModel.Health = 0;
                    alive = false;
                }
            }
        }

        private void CharacterHealthCheck()
        {
            alive = CharacterModel.Health > 0 ? true : false;
        }
        #endregion
    }
}
