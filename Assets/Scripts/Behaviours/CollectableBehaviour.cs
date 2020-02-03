using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType { Red, Blue };

public class CollectableBehaviour : MonoBehaviour
{
    [SerializeField]
    private CollectableType m_type;
    public CollectableType Type => m_type;

    [SerializeField]
    private List<Renderer> m_renderer;

    public void SetVisibility(float transparency)
    {
        m_renderer.ForEach((a) =>
        {
            Color color = a.material.color;
            color.a = transparency;
            a.material.color = color;
        });
    }
}
