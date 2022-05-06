using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathGeneratorScript
{
    /// <summary>
    /// Generates a random set of given number of points on a given ranges of X and Z, while Y remains a given constant. 
    /// </summary>
    /// <returns></returns>
    public static List<Vector3> GenerateRandomPath(float xStart, float xEnd,float yValue, float zStart, float zEnd, int dotAmount )
    {
        var path = new List<Vector3>();
        
        for (int i = 0; i < dotAmount; i++)
        {
            path.Add(new Vector3(Random.Range(xStart, xEnd), yValue, Random.Range(zStart, zEnd)));
        }

        IEnumerable<Vector3> sorted = path.OrderBy(v => v.x).ThenBy(v => v.z);

        return sorted.ToList();
    }

    /// <summary>
    /// Generates the sequence of movements for ball based on a given trajectory to be completed in a given time
    /// </summary>
    /// <param name="trajectory"></param>
    /// <param name="transform"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Sequence GenerateSequence(List<Vector3> trajectory, Transform transform, float time) 
    {
        Sequence sequence = DOTween.Sequence();       
        CalculateDistances(trajectory, out List<float> distances, out float sumDistance);

        for (int i = 0; i < trajectory.Count; i++)
        {
            sequence.Append(transform.DOMove(trajectory[i], time * distances[i] / sumDistance).SetSpeedBased().SetEase(Ease.Linear));
        }

        return sequence;
    }

    /// <summary>
    /// Calculates the lenght of the trajectory and the lenght of each part of the sequence of movements
    /// </summary>
    /// <param name="trajectory"></param>
    /// <param name="distances"></param>
    /// <param name="sumDistance"></param>
    private static void CalculateDistances(List<Vector3> trajectory, out List<float> distances, out float sumDistance)
    {
        distances = new List<float>();
        sumDistance = 0;
        for (int i = 0; i < trajectory.Count - 1; i++)
        {
            distances.Add(Vector3.Distance(trajectory[i], trajectory[i + 1]));
            sumDistance += Vector3.Distance(trajectory[i], trajectory[i + 1]);
        }

        //Removing the first point from the trajectory here, because
        //the object is already there, but we needed that point to calculate the distance of the first movement
        trajectory.RemoveAt(0);
    }
}
