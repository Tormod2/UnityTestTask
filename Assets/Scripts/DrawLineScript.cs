using System.Collections.Generic;
using UnityEngine;

public static class DrawLineScript 
{
    /// <summary>
    /// Sets positions for a given line based on a given trajectory
    /// </summary>
    /// <param name="trajectory"></param>
    /// <param name="line"></param>
    public static void DrawLine(List<Vector3> trajectory,LineRenderer line)
    {
        //Clear LineRenderer before setting new values
        line.positionCount = 0;

        line.positionCount = trajectory.Count;        
        line.SetPositions(trajectory.ToArray());
    }
}
