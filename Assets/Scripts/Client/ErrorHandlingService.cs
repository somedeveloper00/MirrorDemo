using System;
using UnityEngine;

namespace MirrorDemo.Client
{
    /// <summary>
    /// Contains error info for the client, regardless of where the client is.
    /// </summary>
    public sealed class ErrorHandlingService : MonoBehaviour
    {
        /// <summary>
        /// The current error. If empty, there is no error
        /// </summary>
        public (ErrorType type, string message)? Error { get; private set; }

        public event Action ErrorChanged;

        public void SetError(ErrorType type, string message)
        {
            Error = (type, message);
            ErrorChanged?.Invoke();
        }

        public void Dismiss()
        {
            Error = default;
            ErrorChanged?.Invoke();
        }

        public enum ErrorType
        {
            Info,
            Warning,
            Error,
        }
    }
}