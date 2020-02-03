using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class SpawnerBehaviour : MonoBehaviour
{
    [SerializeField]
    private List<SpawnableResource> m_spawnables;

    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < m_spawnables.Count; i++)
        {
            SpawnableResource spawnable = m_spawnables[i];
            int totalAmount = Random.Range(1, spawnable.Locations.Count / 2);
            if (totalAmount == 0) { totalAmount = 1; }

            RandomSpawn(spawnable, 0, totalAmount);
        }
    }
    
    public void Respawn(InteractableType resourceType)
    {
        SpawnableResource spawnable = m_spawnables.Find(a => a.Interactable.Type == resourceType);
        int currentAmount = spawnable.Locations.FindAll(a => a.childCount != 0).Count;

        RandomSpawn(spawnable, currentAmount, currentAmount + 1);
    }

    public Transform FindClosest(InteractableType resourceType, Vector3 position)
    {
        SpawnableResource spawnable = m_spawnables.Find(a => a.Interactable.Type == resourceType);
        return spawnable.Locations.FindAll(a => a.childCount != 0).OrderBy(x => Vector2.Distance(position, x.transform.position)).First();
    }

    private void RandomSpawn(SpawnableResource resource, int current, int total)
    {
        while (current < total)
        {
            List<Transform> empty = resource.Locations.FindAll(a => a.childCount == 0);
            Transform location = empty.ElementAt(Random.Range(0, empty.Count));
            Instantiate(resource.Interactable, location);
            current++;
        }
    }
}

[Serializable]
public class SpawnableResource
{
    [SerializeField]
    private string name;
    [SerializeField]
    private InteractableBehaviour m_interactable;
    public InteractableBehaviour Interactable => m_interactable;

    [SerializeField]
    private List<Transform> m_locations;
    public List<Transform> Locations => m_locations;
}
