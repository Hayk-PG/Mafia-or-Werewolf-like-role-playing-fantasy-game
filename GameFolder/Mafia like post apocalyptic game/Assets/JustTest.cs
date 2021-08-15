using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTest: MonoBehaviour
{
    public _Test test;


    void Start()
    {
        test = new _Test { a = 5, b = 15 };
        CallbackTest(A, B);
    }

    void CallbackTest(Action<int> A, Action B)
    {
        A(new int { });
        B();
    }

    void A(int P)
    {
        P = 5;
    }

    void B()
    {

    }
}

[Serializable]
public class _Test
{
    public int a;
    public int b;
}
