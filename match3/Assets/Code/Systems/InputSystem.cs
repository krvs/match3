using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(InputSystem))]
public sealed class InputSystem : UpdateSystem
{
    private Filter _inputFilter;
    private Filter _cellFilter;
    
    private Vector3 _startPosition;
    
    public override void OnAwake()
    {
        _inputFilter = World.Filter.With<InputData>();
        _cellFilter = World.Filter.With<Cell>();
    }

    public override void OnUpdate(float deltaTime) {
        if (InputUtil.GetInputDown())
        {
            _startPosition = Camera.main.ScreenToWorldPoint(new Vector3(InputUtil.GetInputPosition().x, InputUtil.GetInputPosition().y, 7));
        }
        else if (InputUtil.GetInputUp())
        {
            var inputPos = Camera.main.ScreenToWorldPoint(new Vector3(InputUtil.GetInputPosition().x, InputUtil.GetInputPosition().y, 7));
            var inputDelta = inputPos - _startPosition;

            var inputData = _inputFilter.Select<InputData>();
            ref var inputDataComponent = ref inputData.GetComponent(0);
            
            //Нужна ли тут физика?
            inputDataComponent.Entity = GetCellFromInput(_startPosition);
            inputDataComponent.DeltaInput = inputDelta;
        }
    }

    private IEntity GetCellFromInput(Vector3 startPosition)
    {
        foreach (var entity in _cellFilter)
        {
            var cell = entity.GetComponent<Cell>();
            if (Mathf.RoundToInt(cell.Transform.position.x) == Mathf.RoundToInt(startPosition.x) &&
                Mathf.RoundToInt(cell.Transform.position.y) == Mathf.RoundToInt(startPosition.y))
                return entity;
        }

        return null;
    }
}