using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Random = Unity.Mathematics.Random;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CellMatchSystem))]
public sealed class CellMatchSystem : UpdateSystem
{
    public CellConfig Config;
    private bool _secondMatch;
    private int _matchCount;
    private Random _random;
    
    private Filter _gameStateFilter;
    private Filter _cellFilter;

    private List<IEntity> _matchedEntity;
    
    public override void OnAwake() {
        _random = new Random(12345);
        
        _gameStateFilter = World.Filter.With<GameStateData>();
        _cellFilter = World.Filter.With<Cell>();
    }

    public override void OnUpdate(float deltaTime)
    {
        _matchedEntity = new List<IEntity>();
        
        var gameStateData = _gameStateFilter.Select<GameStateData>();
        ref var gameStateComponent = ref gameStateData.GetComponent(0);
        
        if (gameStateComponent.State != GameState.Match)
            return;
        
        _matchCount = 0;
            
        // find the match
        for (var x = 0; x < Config.MaxCol; x++)
        {
            var cells = _cellFilter.GetCellEntitiesAtColumn(x);
            cells.Sort(new ByCol());
            FindMatches(cells);
        }
            
        for (var y = 0; y < Config.MaxRow; y++)
        {
            var cells = _cellFilter.GetCellEntitiesAtRow(y);
            cells.Sort(new ByRow());
            FindMatches(cells);
        }

        DestroyMatches();

        if (_matchCount == 0)
        {
            if (!_secondMatch)
            {
                //gameState.Hearts--;

                gameStateComponent.State = GameState.Swap;   
                // if (false)
                // {
                //     //AudioUtils.PlaySound(EntityManager, AudioTypes.GameOver);
                //     //gameState.State = GameState.GameOver;
                // }
                // else
                // {
                //     //AudioUtils.PlaySound(EntityManager, GetFailSounds());
                //                          
                // }
            }
            else
            {
                gameStateComponent.State = GameState.Swap;
            }
            _secondMatch = false;
        }
        else
        {
            //AudioUtils.PlaySound(EntityManager, GetMatchSound());
                
            gameStateComponent.State = GameState.Spawn;
            _secondMatch = true;
        }
    }

    private void DestroyMatches()
    {
        foreach (var entity in _matchedEntity)
        {
            var view = entity.GetComponent<CellViewData>();
            Destroy(view.View.gameObject);
            World.RemoveEntity(entity);
        }
        _matchedEntity.Clear();
    }

    private void FindMatches(IReadOnlyList<CellInfo> cells)
    {
        for (var i = 0; i < cells.Count;)
            i = FindOneMatch(cells, i);
    }

    private int FindOneMatch(IReadOnlyList<CellInfo> cells, int startIndex)
    {
        var matchCount = 0;
        var i = startIndex;
        var currentType = cells[i].Cell.Type;
            
        for (;i < cells.Count; i++, matchCount++) 
        {
            if (cells[i].Cell.Type != currentType)
                break;
        }

        if (matchCount >= 3)
            MakeOneMatch(cells, startIndex, i);

        return i;
    }

    private void MakeOneMatch(IReadOnlyList<CellInfo> cells, int startIndex, int end)
    {
        while (startIndex < end)
        {
            _matchCount++;
            _matchedEntity.Add(cells[startIndex++].Entity);
        }
    }
}