using System.Collections.Generic;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(GridInitializer))]
public sealed class GridInitializer : Initializer
{
    [SerializeField] private CellConfig _config;

    public override void OnAwake()
    {
        var gameStateEntity = World.CreateEntity();
        gameStateEntity.SetComponent(new GameStateData()
        {
            Moves = 40,
            State = GameState.Spawn
        });

        var inputEntity = World.CreateEntity();
        inputEntity.AddComponent<InputData>();

        for (var i = 0; i < _config.MaxCol; i++)
        {
            for (var j = 0; j < _config.MaxRow; j++)
            {
                var background = Instantiate(_config.CellBackground);
                background.transform.position = new Vector3(i,j);
            }
        }
    }

    public override void Dispose() {
    }
}