using Services;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Context
{
    public class GameCoreContext : MonoInstaller
    {
        [FormerlySerializedAs("_gameCoreController")] [SerializeField] private GameCoreManager _gameCoreManager;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameCoreManager>().FromInstance(_gameCoreManager).AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<BallServices>().FromNew().AsSingle();
        }
    }
}