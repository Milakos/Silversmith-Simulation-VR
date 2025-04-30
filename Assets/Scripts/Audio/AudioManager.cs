using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

public class AudioManager : IAudioManager
{
    [SerializeField] private EventReference questCompleteEvent;
    public EventReference QuestCompleteEvent => questCompleteEvent;
    
    // public AudioManager(EventReference _questCompleteEvent)
    // {
    //     questCompleteEvent = _questCompleteEvent;
    // }
    
        // Dictionary key struct for tool and surface
    private struct ToolSurfaceKey
    {
        public ToolType Tool;
        public SurfaceType Surface;

        public ToolSurfaceKey(ToolType tool, SurfaceType surface)
        {
            Tool = tool;
            Surface = surface;
        }

        // Override GetHashCode and Equals so the key works in a dictionary.
        public override int GetHashCode()
        {
            return (int)Tool * 397 ^ (int)Surface;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ToolSurfaceKey))
                return false;
            ToolSurfaceKey other = (ToolSurfaceKey)obj;
            return Tool == other.Tool && Surface == other.Surface;
        }
    }
    // Constructor injection: pass in the quest complete event (if used) and the tool-surface mappings.
    // Dictionary mapping a (ToolType, SurfaceType) pair to an audio event.
    private readonly Dictionary<ToolSurfaceKey, EventReference> toolSurfaceAudioDict;
    public AudioManager(EventReference questCompleteEvent, ToolSurfaceAudioMapping[] toolSurfaceMappings)
    {
        this.questCompleteEvent = questCompleteEvent;
        toolSurfaceAudioDict = new Dictionary<ToolSurfaceKey, EventReference>();

        if (toolSurfaceMappings != null)
        {
            foreach (var mapping in toolSurfaceMappings)
            {
                var key = new ToolSurfaceKey(mapping.toolType, mapping.surfaceType);
                if (!toolSurfaceAudioDict.ContainsKey(key))
                {
                    toolSurfaceAudioDict.Add(key, mapping.audioEvent);
                }
            }
        }
    }
    /// <summary>
    /// Retrieves the audio event for a given tool and surface combination.
    /// Returns default if not found.
    /// </summary>
    public EventReference GetToolSurfaceAudioEvent(ToolType tool, SurfaceType surface)
    {
        var key = new ToolSurfaceKey(tool, surface);
        if (toolSurfaceAudioDict.TryGetValue(key, out EventReference evt))
        {
            return evt;
        }
        return default;
    }
    public void PlayOneShot(EventReference evt, Vector3 position)
    {
        if (!evt.IsNull)
        {
            RuntimeManager.PlayOneShot(evt, position);
        }
    }
}
