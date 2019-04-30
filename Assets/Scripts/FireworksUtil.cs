using UnityEngine;

namespace Fireworks {
    public static class Util {
        
    }

    /// <summary>
    /// Provides a coroutine which is interupted by pausing
    /// </summary>
    public class WaitForSecondsUnpaused : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get
            {
                return Time.time >= startTime + waitFor;
            }
        }
        private float waitFor;
        private float startTime;

        public WaitForSecondsUnpaused(float secondsToWait)
        {
            waitFor = secondsToWait;
            startTime = Time.time;
            Debug.Log("why the fuck doesnt it work");
        }
    }
}