﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubInfoManager : MonoBehaviour
{
    public Animation CalenderAnim;
    public Animation CPIAnim;
    
    public TextAnim DayText;
    public TextAnim MonText;
    public TextAnim YearText;

    public TextAnim CPIText;

    bool isDisplay;

    private void Awake()
    {
        DiaplaySub(false);
        CalenderAnim["CalenderOut"].normalizedTime = 1;
        CPIAnim["CPIOut"].normalizedTime = 1;
    }

    internal void DiaplaySub(bool isIn)
    {
        if (isDisplay == isIn)
            return;

        CalenderAnim.CrossFade(isIn ? "CalenderIn" : "CalenderOut");
        CPIAnim.CrossFade(isIn ? "CPIIn" : "CPIOut");

        isDisplay = isIn;
    }
}
