using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PathGeneratorScript
{
    /// <summary>
    /// Generates given number of random points.
    /// </summary>
    /// <param name="number">Amount of points to generate</param>
    /// <returns></returns>
    public static List<Vector3> GenerateRandomPoints(int number)
    {
        var path = new List<Vector3>();

        for (int i = 0; i < number; i++)
        {
            path.Add(new Vector3(Random.Range(-100, 100), 2, Random.Range(-100, 100)));
        }

        path = SortNonIntersecting(path);

        return path;
    }

    private static Vector3 GetCentroid (List<Vector3> points)
    {
        Vector3 centroid = new Vector3();;

        for (var i = 0; i < points.Count; i++)
        {
            centroid.x += points[i].x;
            centroid.z += points[i].z;
        }

        centroid.x /= points.Count;
        centroid.z /= points.Count;

        return centroid;
    }

    private static List<Vector3> SortNonIntersecting (List<Vector3> points)
    {
        var center = GetCentroid(points);
        points.Sort((vector3, vector4) =>
        {
            var angleA = Math.Atan2(vector3.z - center.z, vector3.x - center.x);
            var angleB = Math.Atan2(vector4.z - center.z, vector4.x - center.x);

            if (angleA - angleB < 0)
            {
                return -1;
            }
            if (angleA - angleB > 0)
            {
                return 1;
            }

            return 0;
        });

        return points;
    }

    /// <summary>
    /// Generates the sequence of movements for ball based on a given trajectory to be completed in a given time.
    /// </summary>
    public static Sequence GenerateSequence(List<Vector3> trajectory, Transform transform, float time)
    {
        Sequence sequence = DOTween.Sequence();
        CalculateDistances(trajectory, out List<float> distances, out float sumDistance);

        for (int i = 0; i < trajectory.Count; i++)
        {
            sequence.Append(transform.DOLookAt(trajectory[i], 0f));
            sequence.Append(transform.DOMove(trajectory[i], time * distances[i] / sumDistance).SetSpeedBased().SetEase(Ease.Linear));
        }

        return sequence;
    }

    /// <summary>
    /// Calculates the lenght of the trajectory and the lenght of each part of the sequence of movements.
    /// </summary>
    private static void CalculateDistances(List<Vector3> trajectory, out List<float> distances, out float sumDistance)
    {
        distances = new List<float>();
        sumDistance = 0;
        for (int i = 0; i < trajectory.Count - 1; i++)
        {
            distances.Add(Vector3.Distance(trajectory[i], trajectory[i + 1]));
            sumDistance += Vector3.Distance(trajectory[i], trajectory[i + 1]);
        }

        //Removing the first point from the trajectory here, because the object is already there,
        //but we needed that point to calculate the distance of the first movement.
        trajectory.RemoveAt(0);
    }
}
