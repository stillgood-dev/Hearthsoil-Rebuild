using UnityEngine;

public class TreeFellAnimationRelay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ChoppableTreeController tree;

    private void Awake()
    {
        if (!tree)
        {
            var root = transform.parent;
            tree = root?.Find("Systems/ChoppableTreeController")?.GetComponent<ChoppableTreeController>();
        }
    }

    public void OnFalling()
    {
        tree?.OnTreeFall();
    }

    public void OnTreeFell()
    {
        tree?.OnTreeFell();
    }
}
