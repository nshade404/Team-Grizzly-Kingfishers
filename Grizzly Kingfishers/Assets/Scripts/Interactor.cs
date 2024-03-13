using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionpointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    private void Update()
    {
       // _numFound = Physics.OverlapBoxNonAlloc(_interactionPoint.position,
                                      //         _interactionpointRadius,
                                            //   _colliders,
                                         //      _interactableMask);

    }
}
