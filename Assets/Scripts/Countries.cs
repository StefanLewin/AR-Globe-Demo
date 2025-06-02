using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Countries : ScriptableObject
{
    public CountryFlag[] Flags;
}

[Serializable]
public class CountryFlag
{
    public Country Country;
    public FlagDirection ColorDirection;
    public Color[] FlagColours;
}

public enum FlagDirection
{
    HORIZONTAL,
    VERTICAL
}
