using FMODUnity;
using UnityEngine;

[System.Serializable]
public struct ToolSurfaceAudioMapping
{
    [Tooltip("Select the tool type (e.g., Axe, Hammer, etc.)")]
    public ToolType toolType;

    [Tooltip("Select the surface type (e.g., Wood, Rock, etc.)")]
    public SurfaceType surfaceType;

    [Tooltip("Audio event to play for this tool and surface combination")]
    public EventReference audioEvent;
}
