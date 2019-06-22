using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models.ConditionsAndActions
{
    public abstract class BaseActions: ScriptableObject
    {
        protected Dictionary<string, bool> AllActions { get; private set; }

        #region Действия

        /// <summary>
        /// Персонаж бездействует
        /// </summary>
        public virtual bool Standing
        {
            get
            {
                return AllActions["Standing"];
            }

            set
            {
                AllActions["Standing"] = value;
            }
        }

        /// <summary>
        /// Персонаж бежит
        /// </summary>
        public virtual bool Running
        {
            get
            {
                return AllActions["Standing"];
            }

            set
            {
                AllActions["Standing"] = value;
            }
        }

        /// <summary>
        /// Персонаж идет
        /// </summary>
        public virtual bool Walking
        {
            get
            {
                return AllActions["Standing"];
            }

            set
            {
                AllActions["Standing"] = value;
            }
        }

        /// <summary>
        /// Персонаж падает
        /// </summary>
        public virtual bool Falling
        {
            get
            {
                return AllActions["Falling"];
            }

            set
            {
                AllActions["Falling"] = value;
            }
        }

        #endregion
        
        public BaseActions()
        {
            AllActions = new Dictionary<string, bool>();

            #region Добавляем все действия

            AllActions.Add("Standing", Standing);
            AllActions.Add("Running", Running);
            AllActions.Add("Walking", Walking);
            AllActions.Add("Falling", Falling);

            #endregion

            Standing = true;
        }
    }
}
