using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    void Start()
    {
        SaveSystem.Set("TestListInt", Enumerable.Range(0, 20).ToArray());
    }
}
