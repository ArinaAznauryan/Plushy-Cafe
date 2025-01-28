using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<MyTools>().AsSingle();
        Container.Bind<MyPlayer>().AsSingle();
        //Container.Bind<GameEventsManager>().AsSingle();
        //Container.Bind<GameManager>().AsSingle();
    }
}