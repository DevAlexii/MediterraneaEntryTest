using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallFade : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Material fadeMaterial;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private float sphereRadius = 0.2f; 

    private List<GameObject> wallsHit = new List<GameObject>();

    private void OnEnable()
    {
        GameManager.OnRestartGameCallback += OnRestart;
    }
    private void OnDisable()
    {
        GameManager.OnRestartGameCallback -= OnRestart;
    }

    private void OnRestart()
    {
        wallsHit.Clear();
    }


    void Update()
    {
        UpdateWallTransparency();
    }

    void UpdateWallTransparency()
    {
        foreach (GameObject wall in wallsHit)
        {
            if (wall)
            {
                SetMaterial(wall, defaultMaterial);
            }
        }

        wallsHit.Clear();

        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, sphereRadius, direction, distance);

        foreach (RaycastHit hit in hits)
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Wall"))
            {
                SetMaterial(hitObject, fadeMaterial);

                wallsHit.Add(hitObject);
            }
        }
    }
    void SetMaterial(GameObject obj, Material material)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}