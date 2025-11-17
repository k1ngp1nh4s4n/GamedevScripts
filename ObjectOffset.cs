using UnityEngine;

public class ObjectOffset : MonoBehaviour
{
    [Header("Offset Settings")]
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public Vector3 scaleOffset = Vector3.one;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying && transform.parent != null)
        {
            ApplyOffset();
        }
    }
#endif

    public void ApplyOffset()
    {
        transform.localPosition = positionOffset;
        transform.localEulerAngles = rotationOffset;
        transform.localScale = scaleOffset;
    }
}

