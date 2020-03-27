using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CellDropSystem))]
public sealed class CellDropSystem : UpdateSystem
{ 
    public CellConfig Config;

    private Filter _gameStateFilter;
    private Filter _cellFilter;

    public override void OnAwake()
    {
        _gameStateFilter = World.Filter.With<GameStateData>();
        _cellFilter = World.Filter.With<Cell>();
    }

    public override void OnUpdate(float deltaTime) {
        var gameStateData = _gameStateFilter.Select<GameStateData>();
        ref var gameStateComponent = ref gameStateData.GetComponent(0);

        if (gameStateComponent.State != GameState.Drop)
            return;

        for (var x = 0; x < Config.MaxCol; x++)
        {
            var cells = _cellFilter.GetCellEntitiesAtColumn(x);
            cells.Sort(new ByCol());
                 
            var target = 0;
            foreach (var c in cells)
            {
                var deltaY = c.Cell.Transform.position.y - target;
                if (deltaY > 0)
                {
                    // the check is here instead of in the query, because
                    // we need to count properly even while it is moving
                    // but we don't want to refresh the swap component
                    if (c.Entity.Has<Swap>())
                        continue;
                         
                    c.Entity.SetComponent(new Swap
                    {
                        Position = new Vector2(c.Cell.Transform.position.x, target)
                    });
                }
                target++;
            }
        }

        gameStateComponent.State = GameState.Move;
    }
}