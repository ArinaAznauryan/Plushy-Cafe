using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "AnimalOrderData", order = 1)]
public class AnimalOrderData: ScriptableObject
{
   // [SerializeField] public Order order;
}

// [System.Serializable]
// public class Order {
//     public Food[] foods;
//     public Allergy[] allergies;
// }

// public class AnimalOrder: Order {
//     OrderTime orderTime = new OrderTime();
// }

// [System.Serializable]
// public class Food {
//     public int number;
//     public string name;
// }

// [System.Serializable]
// public class Allergy {
//     public string name;
// }