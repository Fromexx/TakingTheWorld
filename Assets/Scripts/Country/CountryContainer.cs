using System.Collections.Generic;
using UnityEngine;

namespace Country
{
    public class CountryContainer : MonoBehaviour
    {
        [field: SerializeField] public List<Country> Countries { get; private set; }
        [field: SerializeField] public GameObject CountryBallContainerPrefab { get; private set; }
    }
}