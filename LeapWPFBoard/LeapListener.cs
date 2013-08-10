using Leap;
using System;

namespace LeapWPFBoard
{
    public class PointableTracker
    {
        public Vector vector;
        public System.Windows.Media.Color color;

        public PointableTracker(Vector _vector, System.Windows.Media.Color _color)
        {
            vector = _vector;
            color = _color;
        }
    }

    public delegate void TouchFired(PointableTracker _tracker);

    public class LeapListener: Listener
    {
        public event TouchFired FrameFired;
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
        public System.Windows.Media.Color col;

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
                        Pointable pointable = currentFrame.Pointables[0];
                        int alpha = 255;
                        if (pointable.TouchDistance > 0 && pointable.TouchZone != Pointable.Zone.ZONENONE)
                        {
                            alpha = 255 - (int)(255 * pointable.TouchDistance);
                            col = System.Windows.Media.Color.FromArgb((byte)alpha, 0x0, 0xff, 0x0);
                        }
                        else if (pointable.TouchDistance <= 0)
                        {
                            alpha = -(int)(255 * pointable.TouchDistance);
                            col = System.Windows.Media.Color.FromArgb((byte)alpha, 0xff, 0x0, 0x0);
                        }
                        else
                        {
                            alpha = 50;
                            col = System.Windows.Media.Color.FromArgb((byte)alpha, 0x0, 0x0, 0xff);
                        }
                        // Get the velocity of the finger tip
                        var tipVelocity = (int)finger.TipVelocity.Magnitude;

                        // Use tipVelocity to reduce jitters when attempting to hold
                        // the cursor steady
                        if (tipVelocity > 25)
                        {
                            InteractionBox iBox = currentFrame.InteractionBox;
                            lastPos = iBox.NormalizePoint(currentFrame.Hands[0].StabilizedPalmPosition);                            
                        }
                        if (currentFrame.Hands[0].SphereRadius < 80)
                        {
                            ClickedFired(null, null);
                        }
                    }

                }

                previousTime = currentTime;
            }
            FrameFired(new PointableTracker(lastPos, col));
        }
    }
}
