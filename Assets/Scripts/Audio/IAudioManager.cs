using UnityEngine;
using FMODUnity;
public interface IAudioManager
{
    /// <summary>
    /// Plays an audio event at the given position.
    /// </summary>
    void PlayOneShot(EventReference evt, Vector3 position);
    public EventReference GetToolSurfaceAudioEvent(ToolType tool, SurfaceType surface);
    /// <summary>
    /// Returns the quest complete audio event.
    /// This might be a property or a method depending on your design.
    /// </summary>
    EventReference QuestCompleteEvent { get; }
}
