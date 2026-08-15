using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UnityConsent;

namespace PixelPuzzle
{
    public class AnalyticsServiceController : IAnalyticsServiceController
    {
        public async UniTask InitializeAsync(CancellationToken token)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) return;

            try
            {
                await UnityServices.InitializeAsync().AsUniTask().TimeoutWithoutException(TimeSpan.FromSeconds(2f));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            if (!Application.isEditor)
            {
                var consentState = EndUserConsent.GetConsentState();

                if (consentState.AnalyticsIntent != ConsentStatus.Granted)
                {
                    consentState.AnalyticsIntent = ConsentStatus.Granted;
                    EndUserConsent.SetConsentState(consentState);
                }
            }
        }

        public void SendOnPuzzleSolved(int imageID, float solvingTime, int hintsUsed)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer) return;

            CustomEvent puzzleSolvedEvent = new CustomEvent("puzzle_solved")
            {
                {"image_id", imageID},
                {"solving_time", solvingTime},
                {"hints_used", hintsUsed}
            };

            AnalyticsService.Instance.RecordEvent(puzzleSolvedEvent);
            AnalyticsService.Instance.Flush();
        }
    }

    public interface IAnalyticsServiceController
    {
        public UniTask InitializeAsync(CancellationToken token);

        public void SendOnPuzzleSolved(int imageID, float solvingTime, int hintsUsed);
    }
}
