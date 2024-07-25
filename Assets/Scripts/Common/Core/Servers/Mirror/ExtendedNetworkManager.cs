using System;
using Mirror;

namespace MirrorDemo.Common.Core.Mirror
{
    public sealed class ExtendedNetworkManager : NetworkManager
    {
        public event Action Connected;
        public event Action Disconnected;
        public event Action ServerStarted;
        public event Action ClientStarted;
        public event Action<NetworkConnectionToClient> NewClientConnected;
        public event Action<NetworkConnectionToClient> ClientDisconnected;
        public event Action<NetworkConnectionToClient> PlayerAdded;

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Connected?.Invoke();
        }

        public override void OnClientDisconnect()
        {
            base.OnClientDisconnect();
            Disconnected?.Invoke();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            ServerStarted?.Invoke();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            ClientStarted?.Invoke();
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            base.OnServerDisconnect(conn);
            ClientDisconnected?.Invoke(conn);
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            NewClientConnected?.Invoke(conn);
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            PlayerAdded?.Invoke(conn);
        }
    }
}