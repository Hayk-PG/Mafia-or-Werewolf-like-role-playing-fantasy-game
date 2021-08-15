using System;
using System.Collections;
using UnityEngine;

public class SubToEvents: MonoBehaviour
{
    public static SubToEvents instance;


    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region SubscribeToEvents with 1 parameter
    public static void SubscribeToEvents(Action Subscribe)
    {
        instance.SbscrbToEvents(Subscribe);
    }

    void SbscrbToEvents(Action Subscribe)
    {
        StartCoroutine(SbscrbToEventsCor(Subscribe));
    }

    IEnumerator SbscrbToEventsCor(Action Subscribe)
    {
        yield return new WaitUntil(() => PlayerBaseConditions._IsMyGameControllerComponentesNotNull);

        Subscribe();
    }
    #endregion

    #region SubscribeToEvents with 2 parameter
    public static void SubscribeToEvents(bool waitUntil, Action Subscribe)
    {
        instance.SbscrbToEvents(waitUntil, Subscribe);
    }
  
    void SbscrbToEvents(bool waitUntil, Action Subscribe)
    {
        StartCoroutine(SbscrbToEventsCor(waitUntil, Subscribe));
    }
  
    IEnumerator SbscrbToEventsCor(bool waitUntil, Action Subscribe)
    {
        yield return new WaitUntil(() => waitUntil);

        Subscribe();
    }
    #endregion







}
