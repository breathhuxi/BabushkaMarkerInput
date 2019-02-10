using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetPos : DefaultTrackableEventHandler {

	// Public variables
	public GameObject target; // Image Target object
	public OSC osc; // OSC script

	// private
	private bool isDetected;

	// Use this for initialization
	/*void Start () {
		
	}*/

	// Update is called once per frame
	void Update () {

		print (isDetected);
		
		if (isDetected) {

			print (target.name);
			print (target.transform.position);
			print (target.transform.rotation);

			// Send OSC Message
			OscMessage messagePos = new OscMessage();	// Instantiate OSC message object
			messagePos.address = target.name;	// "target": (isDetected, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z)
			messagePos.values.Add(true); //might have bugs
			messagePos.values.Add(target.transform.position.x);				
			messagePos.values.Add(target.transform.position.y);	
			messagePos.values.Add(target.transform.position.z);	
			messagePos.values.Add(target.transform.rotation.x);				
			messagePos.values.Add(target.transform.rotation.y);	
			messagePos.values.Add(target.transform.rotation.z);
			osc.Send(messagePos);
		}

	}

	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		base.OnTrackableStateChanged (previousStatus, newStatus);

		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			isDetected = true;
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			isDetected = false;

			// Send OSC Message
			OscMessage messagePos = new OscMessage();	// Instantiate OSC message object
			messagePos.address = target.name;	// "target": (isDetected, position.x, position.y, position.z, rotation.x, rotation.y, rotation.z)
			messagePos.values.Add(false); //might have bugs
			osc.Send(messagePos);
		}
	}
}
