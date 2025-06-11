using System;
using UnityEngine;

namespace ARGlobe
{
    [CreateAssetMenu]
    public class Countries : ScriptableObject
    {
        public CountryFlag[] Flags;

        public CountryFlag GetFlagByCountry(Country country)
        {
            foreach (var flag in Flags)
            {
                if (flag.Country == country)
                {
                    return flag;
                }
            }
            return null;
        }
    }

    [Serializable]
    public class CountryFlag
    {
        public Country Country;
        public Direction ColorDirection;
        public Color[] FlagColours;
    }

    public enum Direction
    {
        HORIZONTAL,
        VERTICAL
    }

    public enum Country{
        ESTONIA,
        LITHUANIA,
        ITALY,
        BELGIUM,
        GUINEA,
        ARMENIA,
        AUSTRIA,
        BULGARIA,
        GERMANY,
        HUNGARY,
        FRANCE
    }
}