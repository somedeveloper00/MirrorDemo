using MirrorDemo.Client.Common;
using TMPro;
using UnityEngine;

namespace MirrorDemo.Client
{
    /// <summary>
    /// A view for <see cref="ErrorHandlingService"/>
    /// </summary>
    public sealed class ErrorView : MonoBehaviour
    {
        public ErrorHandlingService errorHandlingService;
        [SerializeField] private Container mainContainer;
        [SerializeField] private Container errorContainer;
        [SerializeField] private Container warningContainer;
        [SerializeField] private Container infoContainer;
        [SerializeField] private TMP_Text messageText;

        private void Awake()
        {
            errorHandlingService.ErrorChanged += OnErrorChanged;
        }

        public void DismissClicked() => errorHandlingService.Dismiss();

        private void OnErrorChanged()
        {
            UpdateViewFromError(errorHandlingService.Error);
        }

        private void UpdateViewFromError((ErrorHandlingService.ErrorType type, string message)? error)
        {
            mainContainer.Activated = error.HasValue;
            if (error.HasValue)
            {
                infoContainer.Activated = error.Value.type == ErrorHandlingService.ErrorType.Info;
                warningContainer.Activated = error.Value.type == ErrorHandlingService.ErrorType.Warning;
                errorContainer.Activated = error.Value.type == ErrorHandlingService.ErrorType.Error;
                messageText.text = error.Value.message;
            }
        }
    }
}