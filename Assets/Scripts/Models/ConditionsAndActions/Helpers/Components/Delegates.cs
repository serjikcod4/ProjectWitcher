using EnemySpace;

namespace Assets.Scripts.Models.ConditionsAndActions.Helpers.Components
{
    public delegate void StatusProperty(ConditionArgs Args);

    public delegate void ConditionsUpdate(ref BaseCharacterModel CharacterModel, ref EnemySpecifications enemySpecifications, float deltaTime);
}
