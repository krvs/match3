using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[CreateAssetMenu]
public class CellConfig : ScriptableObject
{
    public List<CellType> CellTypes;
    public GameObject CellBackground;
    public CellView CellView;
    public int MaxCol;
    public int MaxRow;
    
    private Random _random = new Random(12345);
    
    public CellType GetRandomCellType()
    {
        return CellTypes[_random.NextInt(0, CellTypes.Count - 1)];
    }
}
