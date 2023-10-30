using UnityEngine;

public class TaggedObject : MonoBehaviour
{
    [SerializeField] private string initialTagName; // Tag initial de l'objet.
    [SerializeField] private TagManager tagManager;
    public string tagDynamic;

    private void Start()
    {   
        tagManager = GameObject.FindGameObjectWithTag("TagsManager").GetComponent<TagManager>();
    }
}

