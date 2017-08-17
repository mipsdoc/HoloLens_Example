using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.SpatialMapping;
using HoloToolkit.Unity;

public class SpatialMappingNavMesh : MonoBehaviour
{
    public GameObject SpatialMapping;

    void Awake()
    {
        var spatialMappingSources = SpatialMapping.GetComponents<SpatialMappingSource>();
        foreach (var source in spatialMappingSources)
        {
            source.SurfaceAdded += OnSurfaceAdded;
            source.SurfaceUpdated += OnSurfaceUpdated;
        }
    }

    void OnSurfaceAdded(object sender, DataEventArgs<SpatialMappingSource.SurfaceObject> e)
    {
        e.Data.Object.AddComponent<NavMeshSourceTag>();
    }

    void OnSurfaceUpdated(object sender, DataEventArgs<SpatialMappingSource.SurfaceUpdate> e)
    {
        var navMeshSourceTag = e.Data.New.Object.GetComponent<NavMeshSourceTag>();
        if (navMeshSourceTag == null)
        {
            e.Data.New.Object.AddComponent<NavMeshSourceTag>();
        }
    }
}
