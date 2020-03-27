using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UpdateUiSystem))]
public sealed class UpdateUiSystem : UpdateSystem
{
    private Filter _gameStateFilter;
    private Filter _uiFilter;
    
    public override void OnAwake()
    {
        _gameStateFilter = World.Filter.With<GameStateData>();
        _uiFilter = World.Filter.With<UIData>();
    }

    public override void OnUpdate(float deltaTime)
    {
        var gameStateBag = _gameStateFilter.Select<GameStateData>();
        var gameState = gameStateBag.GetComponent(0);
        var UIBag = _uiFilter.Select<UIData>();
        var UIData = UIBag.GetComponent(0);
        UIData.Ui.SetLeftTurns(gameState.Moves);
    }
}