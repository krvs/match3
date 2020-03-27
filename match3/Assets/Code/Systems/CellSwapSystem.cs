using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CellSwapSystem))]
public sealed class CellSwapSystem : UpdateSystem {
    private Filter _gameStateFilter;
    private Filter _inputFilter;
    private Filter _cellFilter;
    
    public override void OnAwake() {
        _gameStateFilter = World.Filter.With<GameStateData>();
        _inputFilter = World.Filter.With<InputData>();
        _cellFilter = World.Filter.With<Cell>();
    }

    public override void OnUpdate(float deltaTime) {
        var gameStateData = _gameStateFilter.Select<GameStateData>();
        ref var gameState = ref gameStateData.GetComponent(0);

        if (gameState.State != GameState.Swap)
            return;
        
        var inputBag = _inputFilter.Select<InputData>();
        ref var inputData = ref inputBag.GetComponent(0);
        if (inputData.Entity == null)
             return;
        
        var inputDelta = inputData.DeltaInput;
        var cellA = inputData.Entity.GetComponent<Cell>();
        var posA = cellA.Transform.position;

        Vector2 posB;
        IEntity entityB = null;
            
        // what is the other cell
        if (math.abs(inputDelta.x) > math.abs(inputDelta.y))
        {
            if (inputDelta.x > 0) // move right
            {
                posB = new Vector2(posA.x + 1, posA.y);
            }
            else // move left
            {
                posB = new Vector2(posA.x - 1, posA.y);
            }
        }
        else
        {
            if (inputDelta.y > 0) // move up
            {
                posB = new Vector2(posA.x, posA.y + 1);
            }
            else // move down
            {
                posB = new Vector2(posA.x, posA.y - 1);
            }
        }
        
        entityB = Find(posB);
        if (entityB == null)
            return;
            
        inputData.Entity.SetComponent(new Swap
        {
            Position = posB
        });
        
        entityB.SetComponent(new Swap
        {
            Position = posA
        });
            
        gameState.State = GameState.Move;
        gameState.Moves--;
        inputData = new InputData();
    }
    
    private IEntity Find(Vector2 pos)
    {
        IEntity entity = null;
        foreach (var e in _cellFilter)
        {
            var c = e.GetComponent<Cell>();
            if(pos.x == c.Transform.position.x && pos.y == c.Transform.position.y)
                entity = e;
        }
        return entity;
    }
}