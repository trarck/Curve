
using System.Collections.Generic;
using UnityEngine;

namespace curve
{ 
    public interface ICurve
    {
        float Length { get; }
        Vector3 GetPointAt(float t);
        List<Vector3> GetPoints();
    }
}