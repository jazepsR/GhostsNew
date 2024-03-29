using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class LocationController : MonoBehaviour
{
    public UnityAction<float> onLoactionReached;
    public UnityAction<float> onLocationExited;

    public LocationSettings locationSettings;

    public bool IsRunning { get; private set; }
    public bool IsTooFar { get; private set; }
    public float Distance { get; private set; }

    private void Start()
    {
        IsRunning = false;
        StartCoroutine(StartLocationService());
    }

    private void Update()
    {
        if(IsRunning)
        {
            float dist = Input.location.lastData.DistanceTo(locationSettings.location);
            //Debug.Log("[" + Input.location.lastData.latitude + "," + Input.location.lastData.longitude + "]");
            //m_distanceText.text = (int)dist + "m";

            Distance = dist;

            if(dist <= locationSettings.distance || Debug.isDebugBuild)
            {
                IsTooFar = false;
                Input.location.Stop();
                IsRunning = false;
                onLoactionReached?.Invoke(dist);
            }
            else
            {
                IsTooFar = true;
                onLocationExited?.Invoke(dist);
            }
        }
        else
        {
            //m_distanceText.text = "Location service " + Input.location.status.ToString();
        }
    }

    private IEnumerator StartLocationService()
    {
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }

        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location disabled by user");
            yield return null;
        }

        Input.location.Start();

        int maxWait = 20;
        var waitASecond = new WaitForSeconds(1);

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return waitASecond;
            maxWait--;
        }

        if(maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            IsRunning = true;
        }
    }
}

public static class LocationInfoExtensions
{
    /// <summary>
    /// Calculate distance in meters between this lat and lon and target lat and lon. Accounts for earth curvature.
    /// </summary>
    /// <param name="target">Target GPS location.</param>
    /// <returns>Distance in meters.</returns>
    public static float DistanceTo(this LocationInfo info, LocationInfo target)
    {
        var lat1 = info.latitude;
        var lon1 = info.longitude;
        var lat2 = target.latitude;
        var lon2 = target.longitude;

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
          Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
          Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        double c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        double distance = R * c;
        distance = distance * 1000; // Convert to meters

        return (float)distance;
    }

    /// <summary>
    /// Calculate distance in meters between this lat and lon and target lat and lon. Accounts for earth curvature.
    /// </summary>
    /// <param name="target">Target GPS location.</param>
    /// <returns>Distance in meters.</returns>
    public static float DistanceTo(this LocationInfo info, Vector2 target)
    {
        var lat1 = info.latitude;
        var lon1 = info.longitude;
        var lat2 = target.x;
        var lon2 = target.y;

        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Mathf.PI / 180 - lat1 * Mathf.PI / 180;
        var dLon = lon2 * Mathf.PI / 180 - lon1 * Mathf.PI / 180;

        float a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) +
          Mathf.Cos(lat1 * Mathf.PI / 180) * Mathf.Cos(lat2 * Mathf.PI / 180) *
          Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2);

        double c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        double distance = R * c;
        distance = distance * 1000; // Convert to meters

        return (float)distance;
    }

    public static float BearingTo(this LocationInfo info, LocationInfo target)
    {
        var lat1 = info.latitude;
        var long1 = info.longitude;
        var lat2 = target.latitude;
        var long2 = target.longitude;

        lat1 *= Mathf.Deg2Rad;
        lat2 *= Mathf.Deg2Rad;
        long1 *= Mathf.Deg2Rad;
        long2 *= Mathf.Deg2Rad;

        float dLon = (long2 - long1);
        float y = Mathf.Sin(dLon) * Mathf.Cos(lat2);
        float x = (Mathf.Cos(lat1) * Mathf.Sin(lat2)) - (Mathf.Sin(lat1) * Mathf.Cos(lat2) * Mathf.Cos(dLon));
        float bearing = Mathf.Atan2(y, x);
        bearing = Mathf.Rad2Deg * bearing;
        bearing = (bearing + 360) % 360;
        bearing = 360 - bearing;

        return bearing;
    }

    public static float BearingTo(this LocationInfo info, float lat, float lon)
    {
        var lat1 = info.latitude;
        var long1 = info.longitude;

        lat1 *= Mathf.Deg2Rad;
        lat *= Mathf.Deg2Rad;
        long1 *= Mathf.Deg2Rad;
        lon *= Mathf.Deg2Rad;

        float dLon = (lon - long1);
        float y = Mathf.Sin(dLon) * Mathf.Cos(lat);
        float x = (Mathf.Cos(lat1) * Mathf.Sin(lat)) - (Mathf.Sin(lat1) * Mathf.Cos(lat) * Mathf.Cos(dLon));
        float bearing = Mathf.Atan2(y, x);
        bearing = Mathf.Rad2Deg * bearing;
        bearing = (bearing + 360) % 360;
        bearing = 360 - bearing;

        return bearing;
    }
}
