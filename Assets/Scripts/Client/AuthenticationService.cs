using System;
using UnityEngine;

namespace MirrorDemo.Client.Client
{
    /// <summary>
    /// Provides data and functionality for authentication
    /// </summary>
    public sealed class AuthenticationService : MonoBehaviour
    {
        public string Token { get; private set; }

        private void Awake()
        {
            Token = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(Token))
            {
                Token = PlayerPrefs.GetString("token", Guid.NewGuid().ToString());
            }
        }
    }
}