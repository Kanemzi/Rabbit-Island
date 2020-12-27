using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    [Header("References")]
    public RabbitData Data;

    public List<CarrotController> GetCarrotsInSight()
	{
        float distance = Data.PerceptionMaxDistance;
        List<CarrotController> results = new List<CarrotController>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);

        foreach (Collider collider in colliders)
		{
            CarrotController carrot = collider.GetComponent<CarrotController>();
            if (carrot && carrot.State != CarrotController.GrowState.Rotten)
                results.Add(carrot);
		}

        return results;
	}

    public List<RabbitController> GetRabbitsInSight()
    {
        float distance = Data.PerceptionMaxDistance;
        List<RabbitController> results = new List<RabbitController>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);

        foreach (Collider collider in colliders)
        {
            RabbitController rabbit = collider.GetComponent<RabbitController>();
            if (rabbit)
                results.Add(rabbit);
        }

        return results;
    }
}
