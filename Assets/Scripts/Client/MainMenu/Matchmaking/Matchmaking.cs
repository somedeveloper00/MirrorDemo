using UnityEngine;

namespace MirrorDemo.Client.MainMenu
{
    public abstract class MatchmakingService : MonoBehaviour
    {
        public bool loading { get; protected set; }
        public CurrentMatch? currentMatch { get; protected set; }

        public abstract void StartMatchmaking();

        public abstract void CancelMatchmaking();

        public struct CurrentMatch
        {
            public string host;
            public string token;
        }
    }
}