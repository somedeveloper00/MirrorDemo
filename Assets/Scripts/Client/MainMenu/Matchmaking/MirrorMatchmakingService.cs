using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MirrorDemo.Client.Client;
using MirrorDemo.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace MirrorDemo.Client.MainMenu
{
    public sealed class MirrorMatchmakingService : MatchmakingService
    {
        public ErrorHandlingService errorHandlingService;
        public AuthenticationService authService;
        public string host = "http://127.0.0.1:8000";
        public string getMatchmakingStatusRoute = "/matchmaking/get/";
        public string startMatchmakingRoute = "/matchmaking/new/";
        public string cancelMatchmakingRoute = "/matchmaking/cancel/";
        public int timeout = 10_000;

        private void Start()
        {
            GetCurrentMatchmakingState();
        }

        private async void GetCurrentMatchmakingState()
        {
            var res = await Post<CurrentMatchStatusResponse, GetMatchmakingStatusRequest>(getMatchmakingStatusRoute, new()
            {
                userToken = authService.Token
            });
            if (res.success)
            {
                SetCurrentMatchFromServerResponse(res.value);
            }
        }

        public override async void StartMatchmaking()
        {
            var res = await Post<CurrentMatchStatusResponse, StartMatchmakingRequest>(startMatchmakingRoute, new()
            {
                userToken = authService.Token
            });
            if (res.success)
            {
                SetCurrentMatchFromServerResponse(res.value);
            }
        }

        public override async void CancelMatchmaking()
        {
            var res = await Post<CurrentMatchStatusResponse, StartMatchmakingRequest>(cancelMatchmakingRoute, new()
            {
                userToken = authService.Token
            });
            if (res.success)
            {
                SetCurrentMatchFromServerResponse(res.value);
            }
        }

        private void SetCurrentMatchFromServerResponse(CurrentMatchStatusResponse value)
        {
            if (string.IsNullOrEmpty(value.matchHost))
            {
                currentMatch = null;
            }
            else
            {
                currentMatch = new()
                {
                    host = value.matchHost,
                    token = value.matchToken
                };
            }
        }

        private async Task<Result<TResult>> Post<TResult, TRequest>(string route, TRequest data)
        {
            loading = true;
            var json = JsonUtility.ToJson(data);
            string url = host + route;
            var req = new UnityWebRequest
            {
                url = url,
                disposeDownloadHandlerOnDispose = true,
                disposeUploadHandlerOnDispose = true,
                method = HttpMethod.Post.Method,
                downloadHandler = new DownloadHandlerBuffer(),
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json))
                {
                    contentType = "application/json"
                },
                timeout = timeout,
            };

            Debug.LogFormat("request POST \"{0}\" \"{1}\"", url, json);
            var op = req.SendWebRequest();
            while (!op.isDone)
            {
                await Task.Yield();
            }
            loading = false;
            if (req.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string responseJson = req.downloadHandler.text;
                    Debug.LogFormat("response POST \"{0}\" {1} \"{2}\"", url, req.responseCode, responseJson);
                    var response = JsonUtility.FromJson<TResult>(responseJson);
                    return new(response);
                }
                catch (Exception e)
                {
                    errorHandlingService.SetError(ErrorHandlingService.ErrorType.Error, e.Message);
                }
            }
            return default;
        }

        public readonly struct Result<T>
        {
            public readonly bool success;
            public readonly T value;

            public Result(T value)
            {
                success = true;
                this.value = value;
            }
        }
    }
}