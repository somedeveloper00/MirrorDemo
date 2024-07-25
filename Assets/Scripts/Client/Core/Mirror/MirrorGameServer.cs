using kcp2k;
using UnityEngine;

namespace MirrorDemo.Common.Core.Mirror
{
    [RequireComponent(typeof(ExtendedNetworkManager))]
    [RequireComponent(typeof(KcpTransport))]
    public sealed class MirrorGameServer : GameServer
    {
        public string host;
        public ushort port;
        public string token;
        public MirrorTransportData transportData;
        private ExtendedNetworkManager _extendedNetworkManager;
        private KcpTransport _simpleWebTransport;
        private MirrorPlayer _player;

#if !UNITY_SERVER
        private void Awake()
        {
            MirrorPlayer.NewInstantiated += p =>
            {
                _player = p;
            };
            _extendedNetworkManager = GetComponent<ExtendedNetworkManager>();
            _simpleWebTransport = GetComponent<KcpTransport>();
            _extendedNetworkManager.Connected += OnConnected;
            _extendedNetworkManager.Disconnected += OnDisconnected;
            _extendedNetworkManager.ClientStarted += OnClientStarted;
        }

        private void Start()
        {
            _extendedNetworkManager.networkAddress = host;
            _simpleWebTransport.Port = port;
            _extendedNetworkManager.StartClient();
        }

        private void Update()
        {
            if (!_extendedNetworkManager.isNetworkActive)
            {
                return;
            }

            // apply data
            ref var model = ref coreGameManager.model;
            transportData?.Apply(ref model);
            coreGameManager.ViewAndTick(Time.deltaTime);
        }

#endif

        private void OnClientStarted()
        {
            Debug.LogFormat("client started");
            coreGameManager.enabled = true;
        }

        private void OnDisconnected()
        {
            Debug.LogFormat("disconnected from server");
            coreGameManager.enabled = false;
        }

        private void OnConnected()
        {
            Debug.LogFormat("connected to server");
        }

        public override void Move(GameInput gameInput)
        {
            Debug.LogFormat("changing input...");
            _player.gameInput = gameInput;
        }

        public override void Shoot()
        {
            Debug.LogFormat("requesting shoot...");
            _player.Shoot();
        }
    }
}