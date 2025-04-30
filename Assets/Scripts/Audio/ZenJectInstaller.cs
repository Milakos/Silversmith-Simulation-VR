using FMODUnity;
using UnityEngine;
using Zenject;

public class ZenJectInstaller : MonoInstaller
{
    [SerializeField] private BaseStateMachine specificStateMachine;
    [SerializeField] private GarbagePool specificGarbagePool;
    // [SerializeField] private EventReference specificAudioEventReference;
       [Header("Audio Manager Settings")]
    [SerializeField] private EventReference questCompleteEvent;
    [SerializeField] private ToolSurfaceAudioMapping[] toolSurfaceMappings;
    public override void InstallBindings()
    {
        // Container.Bind<IAudioManager>().To<AudioManager>().AsSingle().WithArguments(specificAudioEventReference);;
        Container.Bind<GarbagePool>().FromInstance(specificGarbagePool).AsSingle();
        Container.Bind<BaseStateMachine>().FromInstance(specificStateMachine).AsSingle(); 

        // The WithArguments method passes the parameters to the constructor.
        Container.Bind<IAudioManager>().To<AudioManager>().AsSingle().WithArguments(questCompleteEvent, toolSurfaceMappings);
    }
}