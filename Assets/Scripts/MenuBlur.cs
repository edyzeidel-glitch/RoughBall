using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Blurs the 3D world (Depth of Field) whenever a menu is showing.
public class MenuBlur : MonoBehaviour
{
    public Volume volume;

    private DepthOfField dof;

    void Start()
    {
        if (volume != null) volume.profile.TryGet(out dof);
    }

    void Update()
    {
        if (dof == null || GameManager.I == null) return;
        dof.active = GameManager.I.state != GameManager.State.Playing;
    }
}
