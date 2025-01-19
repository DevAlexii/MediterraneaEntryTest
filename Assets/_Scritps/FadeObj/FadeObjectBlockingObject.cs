using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeObjectBlockingObject : MonoBehaviour
{
    [SerializeField]
    private LayerMask LayerMask;
    private Camera _Camera;
    [SerializeField]
    [Range(0, 1f)]
    private float FadedAlpha = 0.33f;
    [SerializeField]
    private float FadeSpeed = 1;

    [Header("Read Only Data")]
    [SerializeField]
    private List<FadingObject> ObjectsBlockingView = new List<FadingObject>();
    private Dictionary<FadingObject, Coroutine> RunningCoroutines = new Dictionary<FadingObject, Coroutine>();

    private RaycastHit[] Hits = new RaycastHit[10];

    private void Start()
    {
        _Camera = Camera.main;
        StartCoroutine(CheckForObjects());
    }

    private IEnumerator CheckForObjects()
    {
        while (true)
        {
            int hits = Physics.SphereCastNonAlloc(
                _Camera.transform.position,
                .3f,
                (transform.position - _Camera.transform.position).normalized,
                Hits,
                5,
                LayerMask
            );

            if (hits > 0)
            {
                for (int i = 0; i < hits; i++)
                {
                    FadingObject fadingObject = GetFadingObjectFromHit(Hits[i]);

                    if (fadingObject != null && !ObjectsBlockingView.Contains(fadingObject))
                    {
                        if (RunningCoroutines.ContainsKey(fadingObject))
                        {
                            if (RunningCoroutines[fadingObject] != null)
                            {
                                StopCoroutine(RunningCoroutines[fadingObject]);
                            }

                            RunningCoroutines.Remove(fadingObject);
                        }

                        RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectOut(fadingObject)));
                        ObjectsBlockingView.Add(fadingObject);
                    }
                }
            }

            FadeObjectsNoLongerBeingHit();

            ClearHits();

            yield return null;
        }
    }

    private void FadeObjectsNoLongerBeingHit()
    {
        List<FadingObject> objectsToRemove = new List<FadingObject>(ObjectsBlockingView.Count);

        foreach (FadingObject fadingObject in ObjectsBlockingView)
        {
            bool objectIsBeingHit = false;
            for (int i = 0; i < Hits.Length; i++)
            {
                FadingObject hitFadingObject = GetFadingObjectFromHit(Hits[i]);
                if (hitFadingObject != null && fadingObject == hitFadingObject)
                {
                    objectIsBeingHit = true;
                    break;
                }
            }

            if (!objectIsBeingHit)
            {
                if (RunningCoroutines.ContainsKey(fadingObject))
                {
                    if (RunningCoroutines[fadingObject] != null)
                    {
                        StopCoroutine(RunningCoroutines[fadingObject]);
                    }
                    RunningCoroutines.Remove(fadingObject);
                }

                RunningCoroutines.Add(fadingObject, StartCoroutine(FadeObjectIn(fadingObject)));
                objectsToRemove.Add(fadingObject);
            }
        }

        foreach (FadingObject removeObject in objectsToRemove)
        {
            ObjectsBlockingView.Remove(removeObject);
        }
    }

    private IEnumerator FadeObjectOut(FadingObject FadingObject)
    {

        FadingObject.Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        FadingObject.Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        FadingObject.Material.SetInt("_ZWrite", 0);
        FadingObject.Material.SetInt("_Surface", 1);
        FadingObject.Material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        FadingObject.Material.SetShaderPassEnabled("DepthOnly", false);
        FadingObject.Material.SetOverrideTag("RenderType", "Transparent");
        FadingObject.Material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        FadingObject.Material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        float time = 0;

        while (FadingObject.Material.color.a > FadedAlpha)
        {
            FadingObject.Material.color = new Color(
                        FadingObject.Material.color.r,
                        FadingObject.Material.color.g,
                        FadingObject.Material.color.b,
                        Mathf.Lerp(FadingObject.InitialAlpha, FadedAlpha, time * FadeSpeed)
                    );

            time += Time.deltaTime;
            yield return null;
        }

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private IEnumerator FadeObjectIn(FadingObject FadingObject)
    {
        float time = 0;

        while (FadingObject.Material.color.a < FadingObject.InitialAlpha)
        {

            if (FadingObject.Material.HasProperty("_Color"))
            {
                FadingObject.Material.color = new Color(
                    FadingObject.Material.color.r,
                    FadingObject.Material.color.g,
                    FadingObject.Material.color.b,
                    Mathf.Lerp(FadedAlpha, FadingObject.InitialAlpha, time * FadeSpeed)
                );
            }

            time += Time.deltaTime;
            yield return null;
        }


        FadingObject.Material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        FadingObject.Material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        FadingObject.Material.SetInt("_ZWrite", 1);
        FadingObject.Material.SetInt("_Surface", 0);
        FadingObject.Material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        FadingObject.Material.SetShaderPassEnabled("DepthOnly", true);
        FadingObject.Material.SetShaderPassEnabled("SHADOWCASTER", true);
        FadingObject.Material.SetOverrideTag("RenderType", "Opaque");
        FadingObject.Material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
        FadingObject.Material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

        if (RunningCoroutines.ContainsKey(FadingObject))
        {
            StopCoroutine(RunningCoroutines[FadingObject]);
            RunningCoroutines.Remove(FadingObject);
        }
    }

    private void ClearHits()
    {
        System.Array.Clear(Hits, 0, Hits.Length);
    }

    private FadingObject GetFadingObjectFromHit(RaycastHit Hit)
    {
        return Hit.collider != null ? Hit.collider.GetComponent<FadingObject>() : null;
    }

}
