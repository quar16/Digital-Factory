using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartLayer : MonoBehaviour
{
    Cart cart;

    private void Awake()
    {
        cart = transform.parent.GetComponent<Cart>();
    }

    public Layer layer;

    public void OnTriggerEnter(Collider other)
    {
        cart.EventSet(layer, State.ENTER, other.gameObject);
    }
    public void OnTriggerExit(Collider other)
    {
        cart.EventSet(layer, State.EXIT, other.gameObject);
    }
}
