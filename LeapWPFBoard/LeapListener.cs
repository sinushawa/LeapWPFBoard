﻿using System;
using Leap;

namespace LeapWPFBoard
{
    public class LeapListener: Listener
    {
        public event EventHandler FrameFired;
        public event EventHandler ClickedFired;

        public override void OnInit(Controller cntrlr)
        {
        }

        public override void OnConnect(Controller cntrlr)
        {
        }

        public override void OnDisconnect(Controller cntrlr)
        {
        }

        public override void OnExit(Controller cntrlr)
        {
        }

        private long currentTime;
        private long previousTime;
        private long timeChange;
        private Vector lastPos = new Vector(0.5f,0.5f,0.5f);

        public float XDelta = 0;
        public float YDelta = 0;
        public float ZDelta = 0;

        public override void OnFrame(Controller cntrlr)
        {
            // Get the current frame.
            Frame currentFrame = cntrlr.Frame();

            currentTime = currentFrame.Timestamp;
            timeChange = currentTime - previousTime;
           
            if (timeChange > 10000)
            {
                if (!currentFrame.Hands.Empty)
                {
                    // Get the first finger in the list of fingers
                    Finger finger = currentFrame.Fingers[0];
                    // Get the closest screen intercepting a ray projecting from the finger
                    Screen screen = cntrlr.CalibratedScreens.ClosestScreenHit(finger);

                    if (screen != null && screen.IsValid)
                    {
                        // Get the velocity of the finger tip
                        var tipVelocity = (int)finger.TipVelocity.Magnitude;

                        // Use tipVelocity to reduce jitters when attempting to hold
                        // the cursor steady
                        if (tipVelocity > 25)
                        {

                            var xScreenIntersect = screen.Intersect(finger, true).x;
                            var yScreenIntersect = screen.Intersect(finger, true).y;
                            lastPos = currentFrame.Hands[0].PalmPosition.Normalized;
                            XDelta = lastPos.x;
                            YDelta = lastPos.z;
                        }
                        if (currentFrame.Hands[0].SphereRadius < 80)
                        {
                            ClickedFired(null, null);
                        }
                    }

                }

                previousTime = currentTime;
            }
            FrameFired(null, null);
        }
    }
}
