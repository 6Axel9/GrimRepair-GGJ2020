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
    [SerializeField]
    private float m_ceilingHeight;

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
    
    public void Respawn(CollectableType resourceType)
    {
        SpawnableResource spawnable = m_spawnables.Find(a => a.Interactable.Type == resourceType);
        int currentAmount = spawnable.Locations.FindAll(a => a.childCount != 0).Count;

        RandomSpawn(spawnable, currentAmount, currentAmount + 1);
    }

    public Transform FindClosest(CollectableType resourceType, Vector3 position)
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
            CollectableBehaviour collectable = Instantiate(resource.Interactable, location);

            switch (collectable.Type)
            {
                case CollectableType.Red:
                    collectable.SetVisibility(collectable.transform.position.y > m_ceilingHeight ? 0.25f : 1f);
                    break;
                case CollectableType.Blue:
                    collectable.SetVisibility(collectable.transform.position.y > m_ceilingHeight ? 1f : 0.25f);
                    break;
            }

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
    private CollectableBehaviour m_interactable;
    public CollectableBehaviour Interactable => m_interactable;

    [SerializeField]
    private List<Transform> m_locations;
    public List<Transform> Locations => m_locations;
}
