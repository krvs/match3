using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CellSpawnerSystem))]
public sealed class CellSpawnerSystem : UpdateSystem
{
    public CellConfig Config;

    private Filter _gameStateFilter;
    private Filter _cellFilter;
    
    public override void OnAwake()
    {
        _gameStateFilter = World.Filter.With<GameStateData>();
        _cellFilter = World.Filter.With<Cell>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var gameStateData = _gameStateFilter.Select<GameStateData>();
        ref var gameStateComponent = ref gameStateData.GetComponent(0);

        if (gameStateComponent.State != GameState.Spawn)
            return;
        
        for (var x = 0; x < Config.MaxCol; x++)
        {
            var cells = _cellFilter.GetCellEntitiesAtColumn(x);
            var needed = Config.MaxRow - cells.Count;
            
            for(var i = 0; i < needed; i++)
            {
                var y = i + Config.MaxRow;

                var cellEntity = World.CreateEntity();

                var cell = cellEntity.AddComponent<Cell>();
                var cellView = Instantiate(Config.CellView);
                cell.Type = Config.GetRandomCellType();
                cell.Transform = cellView.transform;
                cell.Transform.position = new Vector3(x, y, 0);
                cellEntity.SetComponent(cell);

                var cellViewComponent = cellEntity.AddComponent<CellViewData>();
                cellView.SetupView(cell.Type.Sprite);
                cellViewComponent.View = cellView;
                cellEntity.SetComponent(cellViewComponent);
            }
        }
        
        gameStateComponent.State = GameState.Drop;
    }
}