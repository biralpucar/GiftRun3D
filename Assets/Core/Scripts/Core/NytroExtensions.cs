using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using static NytroExtensionsUtils;

public static class NytroListExtensions
{
    /// <summary>
    /// Randomize a list without creating a copy.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        // fisher-yates shuffle
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = Random.Range(i, count);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    /// <summary>
    /// Try to select and remove a random element from a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns>If the list is empty "false" is returned. 
    /// Otherwise, an element of type <typeparamref name="T"/> is set to <paramref name="element"/>, removed from the list and "true" is returned.</returns>
    public static bool TryPopRandom<T>(this List<T> list, out T element)
    {
        element = default;
        if (list.Count == 0) return false;

        int idx = Random.Range(0, list.Count);
        element = list[idx];

        list.RemoveAt(idx);

        return true;
    }

    /// <summary>
    /// Try to select a random element from a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns>If the list is empty "false" is returned. 
    /// Otherwise, an element of type <typeparamref name="T"/> is set to <paramref name="element"/> and "true" is returned.</returns>
    public static bool TryGetRandom<T>(this List<T> list, out T element)
    {
        element = default;
        if (list.Count == 0) return false;

        int idx = Random.Range(0, list.Count);
        element = list[idx];

        return true;
    }

    /// <summary>
    /// Sort a list of components by their distance to the given position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="position">The position which the distance is calculated.</param>
    /// <param name="start">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    public static void SortByDistance<T>(this List<T> list, Vector3 position, int start, int length) where T : Component
    {
        DistanceComparer<T> comparer = new()
        {
            originPosition = position
        };

        list.Sort(start, length, comparer);
    }

    /// <summary>
    /// Sort a list of components by their distance to the given position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="position">The position which the distance is calculated.</param>
    public static void SortByDistance<T>(this List<T> list, Vector3 position) where T : Component
    {
        SortByDistance(list, position, 0, list.Count);
    }
}

public static class NytroArrayExtensions
{
    /// <summary>
    /// Sort an array of components by their distance to the given position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="position">The position which the distance is calculated.</param>
    /// <param name="start">The starting index of the range to sort.</param>
    /// <param name="length">The number of elements in the range to sort.</param>
    public static void SortByDistance<T>(this T[] array, Vector3 position, int start, int length) where T : Component
    {
        DistanceComparer<T> comparer = new()
        {
            originPosition = position
        };

        System.Array.Sort(array, start, length, comparer);
    }

    /// <summary>
    /// Sort an array of components by their distance to the given position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="position">The position which the distance is calculated.</param>
    public static void SortByDistance<T>(this T[] array, Vector3 position) where T : Component
    {
        SortByDistance(array, position, 0, array.Length);
    }
}

public static class NytroShaderExtensions
{
    /// <summary>
    /// Changes a materials shader to given type. Only switches between NytroToon shaders.
    /// </summary>
    /// <param name="material"></param>
    /// <param name="shaderType">Shader type to switch to.</param>
    public static void SetNytroShader(this Material material, NytroShaderType shaderType)
    {
        switch (shaderType)
        {
            case NytroShaderType.Opaque:
                material.shader = NytroShaderUtilities.opaqueShader;
                break;
            case NytroShaderType.Transparent:
                material.shader = NytroShaderUtilities.transparentShader;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Animate the opacity of a renderer. The material of the renderer must be using NytroToon shaders.
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="to">Final alpha value.</param>
    /// <param name="duration">Duration of the fade.</param>
    /// <param name="callback">An optional callback to be invoked after finishing the fade.</param>
    public static void Fade(this Renderer renderer, float to, float duration, System.Action<Renderer> callback)
    {
        // renderer.material will clone the material for us automatically if it is shared.
        float currentAlpha = renderer.material.GetFloat(NytroShaderUtilities.NytroToonKeywords.alpha);

        // we need a monobehaviour to hold our coroutines
        // since we have a gamemanager instance, we can use that
        GameManager.instance.StartCoroutine(FadeRoutine(renderer, currentAlpha, to, duration, callback));
    }

    public static void FadeOutAndDestroy(this Renderer renderer, float duration)
    {
        renderer.Fade(0, duration, AfterFadingOut);
    }

    private static void AfterFadingOut(Renderer renderer)
    {
        Object.Destroy(renderer.gameObject);
        Object.Destroy(renderer.material);
    }

    private static IEnumerator FadeRoutine(Renderer renderer, float from, float to, float duration, System.Action<Renderer> callback = null)
    {
        Material material = renderer.material;
        material.SetNytroShader(NytroShaderType.Transparent);

        float currentDuration = 0;
        while (currentDuration < duration)
        {
            float t = currentDuration / duration;

            float currentAlpha = Mathf.Lerp(from, to, t);
            material.SetFloat(NytroShaderUtilities.NytroToonKeywords.alpha, currentAlpha);

            currentDuration += Time.deltaTime;
            yield return null;
        }

        material.SetFloat(NytroShaderUtilities.NytroToonKeywords.alpha, to);

        if (Mathf.Approximately(to, 1f) | to > 1f)
        {
            material.SetNytroShader(NytroShaderType.Opaque);
        }

        yield return null;

        callback?.Invoke(renderer);
    }
}

public static class NytroAgentExtensions
{
    /// <summary>
    /// Checks whether agent is at its given destination set by agent.SetDestination()
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="sqrMinimumDistance">Squared minimum distance threshold to check for arrival.</param>
    /// <returns></returns>
    public static bool IsAtDestination(this NavMeshAgent agent, float sqrMinimumDistance = 0.01f)
    {
        Vector3 destination = agent.destination;
        Vector3 position = agent.transform.position;

        Vector3 diff = destination - position;
        return diff.sqrMagnitude < sqrMinimumDistance;
    }

    /// <summary>
    /// Returns a new WaitUntil object that can be yielded until agent reaches its destination. Uses IsAtDestination internally.
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="sqrMinimumDistance">Squared minimum distance threshold to check for arrival.</param>
    /// <returns></returns>
    public static WaitUntil WaitUntilDestination(this NavMeshAgent agent, float sqrMinimumDistance = 0.01f)
    {
        return new WaitUntil(() =>
        {
            return agent.IsAtDestination(sqrMinimumDistance: sqrMinimumDistance);
        });
    }
}

public static class NytroMiscExtensions
{
    /// <summary>
    /// Try to get a component from the owning rigidbody of this collider.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collider"></param>
    /// <param name="component"></param>
    /// <returns>If collider does not have an owning rigidbody or component is not found, false is returned. 
    /// Otherwise, true is returned and <paramref name="component"/> is populated with its reference.</returns>
    public static bool TryGetRigidbodyComponent<T>(this Collider collider, out T component) where T : class
    {
        if (collider.attachedRigidbody == null)
        {
            component = default;
            return false;
        }

        else
        {
            component = collider.attachedRigidbody.GetComponent<T>();
            return component != null;
        }
    }

    /// <summary>
    /// Component-wise multiplication of a quaternion.
    /// </summary>
    /// <param name="input">Quaternion to multiply.</param>
    /// <param name="scalar">Scalar to multiply.</param>
    /// <returns>Multiplied quaternion</returns>
    public static Quaternion ScalarMultiply(this Quaternion input, float scalar)
    {
        return new Quaternion(
            input.x * scalar,
            input.y * scalar,
            input.z * scalar,
            input.w * scalar);
    }
}

public static class NytroLayerMaskExtensions
{
    /// <summary>
    /// Checks whether a LayerMask contains a layer by name.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layerName">Name of the layer to check against the mask.</param>
    /// <returns>true if mask contains the layer, false if not.</returns>
    public static bool HasLayer(this LayerMask layerMask, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);

        if (layer == -1)
        {
            Logger.Log("Layer does not exist: " + layerName);
            return false;
        }

        return layerMask.HasLayer(layer);
    }

    /// <summary>
    /// Checks whether a LayerMask contains a layer by index.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layerIndex">Index of the layer to check against the mask.</param>
    /// <returns>true if mask contains the layer, false if not.</returns>
    public static bool HasLayer(this LayerMask layerMask, int layerIndex)
    {
        int indexMask = 1 << layerIndex;
        return (layerMask & indexMask) != 0;
    }
    
    /// <summary>
    /// Checks whether a LayerMask contains all the specified layer indexes.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layers"></param>
    /// <returns>true if mask contains all the layers, false if not.</returns>
    public static bool HasAllLayers(this LayerMask layerMask, params int[] layers)
    {
        int indexMask = 0;

        for (int i = 0; i < layers.Length; i++)
        {
            int mask = 1 << layers[i];
            indexMask |= mask;
        }

        return (layerMask & indexMask) != 0;
    }

    /// <summary>
    /// Checks whether a LayerMask contains any of the specified layer indexes.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="layers"></param>
    /// <returns>true if any of the provided layer indexes exist in the mask. false if none exists.</returns>
    public static bool HasAnyLayer(this LayerMask layerMask, params int[] layers)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            int layer = layers[i];
            if (layerMask.HasLayer(layer)) return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether a LayerMask contains all of the layers specified by the other mask.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="otherMask"></param>
    /// <returns>true if mask contains all the layers, false if not.</returns>
    public static bool HasAllLayers(this LayerMask layerMask, LayerMask otherMask)
    {
        return (layerMask & otherMask) == otherMask;
    }

    /// <summary>
    /// Checks whether a LayerMask contains any of the layers specified by the other mask.
    /// </summary>
    /// <param name="layerMask"></param>
    /// <param name="otherMask"></param>
    /// <returns>true if any of the layers match. false if none exists.</returns>
    public static bool HasAnyLayer(this LayerMask layerMask, LayerMask otherMask)
    {
        return (layerMask & otherMask) != 0;
    }
}

public static class NytroExtensionsUtils
{
    public struct DistanceComparer<T> : IComparer<T> where T : Component
    {
        public Vector3 originPosition;

        public int Compare(T x, T y)
        {
            Vector3 xPos = x.transform.position;
            Vector3 yPos = y.transform.position;

            float xDist = (xPos - originPosition).sqrMagnitude;
            float yDist = (yPos - originPosition).sqrMagnitude;

            return xDist.CompareTo(yDist);
        }
    }
}