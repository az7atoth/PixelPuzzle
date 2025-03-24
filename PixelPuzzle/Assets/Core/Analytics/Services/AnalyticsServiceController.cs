using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace PixelPuzzle
{
    public class AnalyticsServiceController : IAnalyticsServiceController
    {
        public async UniTask InitializeAsync(CancellationToken token)
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            AnalyticsService.Instance.StartDataCollection();
        }

        public void SendOnPuzzleSolved(int imageID, float solvingTime, int hintsUsed)
        {
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
