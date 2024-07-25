using System;

namespace MirrorDemo.Common
{
    /// <summary>
    /// Client to server. Attempt to get the current state of matchmaking.
    /// </summary>
    [Serializable]
    public struct GetMatchmakingStatusRequest
    {
        public string userToken;
    }

    /// <summary>
    /// Client to server. Attempt to start a new matchmaking.
    /// </summary>
    [Serializable]
    public struct StartMatchmakingRequest
    {
        public string userToken;
    }

    /// <summary>
    /// Server to client. Returns the state of the current match
    /// </summary>
    [Serializable]
    public struct CurrentMatchStatusResponse
    {
        /// <summary>
        /// current match's host. if this is null or empty, user is not in any match
        /// </summary>
        public string matchHost;
        public string matchToken;
    }

    /// <summary>
    /// Client to server. Attempt to cancel an on-going matchmaking
    /// </summary>
    [Serializable]
    public struct CancelMatchmakingRequest
    {
        public string userToken;
    }
}