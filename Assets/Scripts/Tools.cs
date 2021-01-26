using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class Tools
{
    //constants
    private const double CONSTANT = 57.29638;

    //useful parameters
    private static long waitAndDoMethodMilliseconds;
    private static bool hasCheckedMilliseconds = false;


    //audio stuff--------------------------------------------------------------------------

    //this will stop the audio just if called in an update function
    public static void StopAudio(AudioSource audio, float reduceSpeed)
    {
        if (audio.volume > 0)
        {
            audio.volume -= reduceSpeed;
        }
    }


    //math stuff---------------------------------------------------------------------------
    
    //converts a vector3 into an array of floats
    public static float[] Vector3ToArray(Vector3 vector)
    {
        float[] result = new float[3];

        result[0] = vector.x;
        result[1] = vector.y;
        result[2] = vector.z;

        return result;
    }

    //converts a float[] into a vector3
    public static Vector3 ArrayToVector3(float[] array)
    {
        return new Vector3(array[0], array[1], array[2]);
    }

    //converts a vector2 to an angle
    public static float Vector2ToAngle(Vector2 vector)
    {
        float x = vector.x;
        float y = vector.y;

        if (y == 0)
        {
            if (x >= 0)
            {
                return 0;
            }
            else if (x < 0)
            {
                return 180;
            }
        }
        else if (x == 0)
        {
            if (y > 0)
            {
                return 90;
            }
            else if (y < 0)
            {
                return 270;
            }
        }
        else if (x > 0 && y > 0)
        {
            return (float)(Math.Atan(y / x) * CONSTANT);
        }
        else if (x < 0 && y < 0)
        {
            float toReturn = (float)(Math.Atan(y / x) * CONSTANT);
            return toReturn + 180;
        }
        else if (x > 0 && y < 0)
        {
            float toReturn = (float)(Math.Atan((y * -1) / x) * CONSTANT);
            return (90 - toReturn) + 270;
        }
        else if (x < 0 && y > 0)
        {
            float toReturn = (float)(Math.Atan(y / (x * -1)) * CONSTANT);
            return (90 - toReturn) + 90;
        }

        return 0;
    }

    //converts a degree value (like 361°) into its restricted value (1°) (in this case, 361° = 1°)
    public static double RotationToRestrictedRotation(double rotation)
    {
        double absRotation = Math.Abs(rotation);
        double restrictedRotation;

        if (absRotation / 360d <= 1)
        {
            restrictedRotation = absRotation;
        }
        else
        {
            int divisor = Convert.ToInt32(
                Math.Floor(absRotation / 360d));

            restrictedRotation = absRotation - (360 * divisor);
        }

        if (rotation < 0)
        {
            return 360 - restrictedRotation;
        }
        else
        {
            return restrictedRotation;
        }

    }

    //converts a rotation into a vector 2
    public static Vector2 RotationToVector2(double rotation, int xMult, int yMult)
    {
        double realRotation = RotationToRestrictedRotation(rotation);
        double radiantsRotation = realRotation * (Math.PI / 180.0d);
        float xValue;
        float yValue;

        xValue = xMult * (float)Math.Tan(radiantsRotation);
        yValue = yMult * ((float)Math.Pow((float)Math.Tan(radiantsRotation), 2));

        while (Math.Abs(xValue) <= 1 || Math.Abs(yValue) <= 1)
        {
            xValue *= 2;
            yValue *= 2;
        }

        return new Vector2(xValue, yValue);
    }

    //restricts a large vector2 into a smaller one, keeping the rapport between the two
    public static Vector2 RestrictVector(Vector2 baseVector, float requiredSum)
    {
        float sum = Math.Abs(baseVector.x) + Math.Abs(baseVector.y);

        if (Math.Abs(sum) != requiredSum)
        {
            float x = (baseVector.x * requiredSum) / sum;
            float y = (baseVector.y * requiredSum) / sum;

            return new Vector2(x, y);
        }
        else
        {
            return baseVector;
        }
    }


    //useful stuff--------------------------------------------------------------------------
    
    //checks if a button is pushed down and if it is does an action
    public static void CheckAndDo(KeyCode key, Action action)
    {
        if (Input.GetKeyDown(key))
            action();
    }

    //wait method.
    public static void Wait(int milliseconds)
    {
        System.Threading.Thread.Sleep(milliseconds);
    }

    //waits some time and then does an action
    public static void WaitAndDo(long milliseconds, Action action, Stopwatch stopwatch, bool hasToStopTheStopwatch)
    {
        if (!stopwatch.IsRunning)
            throw new InvalidOperationException("the parameter stopwatch needs to be started");

        if (!hasCheckedMilliseconds)
        {
            waitAndDoMethodMilliseconds = stopwatch.ElapsedMilliseconds;
            hasCheckedMilliseconds = true;
        }

        if (stopwatch.ElapsedMilliseconds - waitAndDoMethodMilliseconds > milliseconds)
        {
            action();
            hasCheckedMilliseconds = false;

            if (hasToStopTheStopwatch)
            {
                stopwatch.Stop();
            }
        }
    }

    //generates a random bool
    public static bool RandomBool()
    {
        int x = UnityEngine.Random.Range(0, 2);

        if (x > 0)
            return true;
        else
            return false;
    }
}
