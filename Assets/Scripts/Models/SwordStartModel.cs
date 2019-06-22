using UnityEngine;
using Assets.Scripts.BaseScripts;
using Assets.Scripts.Interfaces;

public class SwordStartModel : MonoBehaviour
{
    //Обычный урон 
    private float damageNormal = 10f;

    //Тяжелый урон
    private float damageHeavy = 30f;

    //Текущий урон
    private float currentDamage;

    //Флаг атаки
    private bool isAttack = true;

    //Флаг обычной атаки
    private bool isNormalAttack;

    //Флаг тяжелой атаки
    private bool isHeavyAttack;

    #region Модификация

    [SerializeField] ItemConditionsChance ItemConditions;

    private void Awake()
    {
        if (ItemConditions != null)
        {
            ItemConditions.SetItemConditionsChance();
        }
    }

    //Метод Передачи статуса
    private void SetConditions(IGetConditions obj)
    {
        //Проверка отсутствия объекта 
        if (obj != null)
        {
            if(ItemConditions.GetCurrentItemConditions().Length != 0)
            {
                obj.ApplyCondition(ItemConditions.GetCurrentItemConditions());
            }
        }
    }

    #endregion
    
    private void OnCollisionEnter(Collision collision)
    {
        //Если коллайдер меча соприкоснулся с игроком то возвращаемся
        if (collision.collider.tag == "Player") return;

        //Если коллайдер меча коснулся любого объекта то проверяем статус атаки 
        GetAttackStatus();

        //Вызываем метод Нанесения урона у всех объектов кто наследует интерфейс Нанесения урона
        SetDamage(collision.collider.GetComponent<ISetDamage>());

        //Вызываем метод передачи Статусов оружия
        SetConditions(collision.collider.GetComponent<IGetConditions>());
    }

    //Метод Нанесения урона
    private void SetDamage(ISetDamage obj)
    {
        //Проверка отсутствия объекта 
        if (obj != null)
        {
            obj.ApplyDamage(currentDamage);
        }
    }
    
    //Метод проверки статуса атаки
    private void GetAttackStatus()
    {
        //Состояние параметра обычной атаки у Аниматора
        isNormalAttack = StartScript.GetStartScript.animController.normaAttack;

        //Состояние параметра тяжелой атаки у Аниматора
        isHeavyAttack = StartScript.GetStartScript.animController.heavyAttack;

        //Если производится Обычная атака то
        if (isAttack)
        {
            //Устанавливаем обычный урон
            currentDamage = damageNormal;
        }
        //Если производится Тяжелая атака то
        if (isAttack)
        {
            //Устанавливаем тяжелый урон
            currentDamage = damageHeavy;
        }
    }
}
