using System;
using System.Collections.Generic;
using UnityEngine;

public static class NytroPhysicsUtilities
{
    private const int BUFFER_SIZE = 1024;

    private static Collider[] colliderCache = new Collider[BUFFER_SIZE];
    private static Dictionary<Type, object> componentCache = new();

    public delegate void ProcessDelegate<T>(T component);

    [Serializable]
    public abstract class BaseOverlapper
    {
        [Header("Params -- Base")]
        public bool sortByDistance;
        public Vector3 center;
        public QueryTriggerInteraction triggerInteraction;
        public LayerMask mask;

        protected abstract int PerformCachedOverlap();

        public ReadOnlySpan<Collider> GetOverlappingColliders()
        {
            int hits = PerformCachedOverlap();

            if (hits == 0) return ReadOnlySpan<Collider>.Empty;

            else
            {
                if (sortByDistance)
                {
                    colliderCache.SortByDistance(center, 0, hits);
                }

                return new ReadOnlySpan<Collider>(colliderCache, 0, hits);
            }
        }

        public ReadOnlySpan<T> GetOverlappingComponents<T>(bool checkRigidbody) where T : Component
        {
            ReadOnlySpan<Collider> colliders = GetOverlappingColliders();
            T[] componentArray = GetCachedComponentArray<T>();

            int j = 0;

            if (checkRigidbody)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if (collider.TryGetRigidbodyComponent(out T component))
                    {
                        componentArray[j] = component;
                        j++;
                    }
                }
            }

            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if (collider.TryGetComponent(out T component))
                    {
                        componentArray[j] = component;
                        j++;
                    }
                }
            }

            if (j == 0) return ReadOnlySpan<T>.Empty;
            else return new ReadOnlySpan<T>(componentArray, 0, j);
        }

        public void FindAndProcessOverlappedColliders(ProcessDelegate<Collider> callback)
        {
            ReadOnlySpan<Collider> colliders = GetOverlappingColliders();
            for (int i = 0; i < colliders.Length; i++)
            {
                Collider collider = colliders[i];
                callback(collider);
            }
        }

        public void FindAndProcessOverlappedComponents<T>(ProcessDelegate<T> callback, bool checkRigidbody) where T : class
        {
            ReadOnlySpan<Collider> colliders = GetOverlappingColliders();

            if (checkRigidbody)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if (collider.TryGetRigidbodyComponent(out T component))
                    {
                        callback(component);
                    }
                }
            }

            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    Collider collider = colliders[i];
                    if (collider.TryGetComponent(out T component))
                    {
                        callback(component);
                    }
                }
            }
        }
    }

    [Serializable]
    public class SphereOverlapper : BaseOverlapper
    {
        [Header("Params -- Sphere")]
        public float radius;

        protected override int PerformCachedOverlap()
        {
            int hits = Physics.OverlapSphereNonAlloc(center, radius, colliderCache, mask, triggerInteraction);
            return hits;
        }
    }

    [Serializable]
    public class BoxOverlapper : BaseOverlapper
    {
        [Header("Params -- Box")]
        public Vector3 halfExtents;
        public Quaternion orientation;

        protected override int PerformCachedOverlap()
        {
            int hits = Physics.OverlapBoxNonAlloc(center, halfExtents, colliderCache, orientation, mask, triggerInteraction);
            return hits;
        }
    }

    private static T[] GetCachedComponentArray<T>() where T : class
    {
        Type componentType = typeof(T);
        T[] componentArray;

        if (componentCache.TryGetValue(componentType, out object array))
        {
            componentArray = array as T[];
        }

        else
        {
            componentArray = new T[BUFFER_SIZE];
            componentCache[componentType] = componentArray;
        }

        return componentArray;
    }
}
