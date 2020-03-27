using System.Collections;
using System.Collections.Generic;
using Morpeh;
using UnityEngine;

static class CellUtil
{
    public static List<CellInfo> GetCellEntitiesAtColumn(this Filter filter, int column)
    {
        var result = new List<CellInfo>();

        foreach (var entity in filter)
        {
            var cell = entity.GetComponent<Cell>();
            if (Mathf.RoundToInt(cell.Transform.position.x) == column)
            {
                result.Add(new CellInfo
                {
                    Cell = cell,
                    Entity = entity
                });
            }
        }
            
        return result;
    }
        
    public static List<CellInfo> GetCellEntitiesAtRow(this Filter filter, int row)
    {
        var result = new List<CellInfo>();

        foreach (var entity in filter)
        {
            var cell = entity.GetComponent<Cell>();
            if (Mathf.RoundToInt(cell.Transform.position.y) == row)
            {
                result.Add(new CellInfo
                {
                    Cell = cell,
                    Entity = entity
                });
            }
        }
            
        return result;
    }
}

class ByCol : IComparer<CellInfo>
{
    public int Compare(CellInfo x, CellInfo y)
    {
        return Mathf.RoundToInt(x.Cell.Transform.position.y) - Mathf.RoundToInt(y.Cell.Transform.position.y);
    }
}
    
class ByRow : IComparer<CellInfo>
{
    public int Compare(CellInfo x, CellInfo y)
    {
        return Mathf.RoundToInt(x.Cell.Transform.position.x) - Mathf.RoundToInt(y.Cell.Transform.position.x);
    }
}