using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CellMoveSystem))]
public sealed class CellMoveSystem : UpdateSystem {
    private const float _ProgressSpeed = 1.5f;
    private const float _ProgressDonePercentage = 0.8f;
    
    private Filter _gameStateFilter;
    private Filter _cellFilter;
    
    public override void OnAwake() {
        _gameStateFilter = World.Filter.With<GameStateData>();
        _cellFilter = World.Filter.With<Cell>().With<Swap>();
    }

    public override void OnUpdate(float deltaTime) {
        var gameStateData = _gameStateFilter.Select<GameStateData>();
        ref var gameStateComponent = ref gameStateData.GetComponent(0);
        
        if (gameStateComponent.State != GameState.Move)
            return;

        var somethingMoved = false;

        foreach (var entity in _cellFilter)
        {
            var swap = entity.GetComponent<Swap>();
            var cell = entity.GetComponent<Cell>();
            swap.Progress += deltaTime * _ProgressSpeed;
            entity.SetComponent(swap);
            if (swap.Progress < _ProgressDonePercentage)
            {
                somethingMoved = true;
                var dir = (swap.Position - cell.Transform.position) * swap.Progress;
                cell.Transform.position += dir;
            }
            else
            {
                cell.Transform.position = swap.Position;
                entity.RemoveComponent<Swap>();
            }
        }

        if (somethingMoved) 
            return;
            
        gameStateComponent.State = GameState.Match;
    }
}