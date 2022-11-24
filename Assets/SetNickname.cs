using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class SetNickname : MonoBehaviour
{
    //private Button button;
    FirebaseFirestore db;
    void Start()
    {
	db = FirebaseFirestore.DefaultInstance;
//    	button.onClick.AddListener(RunThisTask);
    }
/*    void RunThisTask(){
	DocumentReference docRef = db.Collection("1").Document();
      	Dictionary<string, object> user = new Dictionary<string, object>
      	{
              	{ "Nickname", "stevne"},
              	{ "Cleartime", 3.3},
      	};
      	docRef.SetAsync(user).ContinueWithOnMainThread(task => {
              	Debug.Log("Added data to the alovelace document in the users collection.");
      	});
    }*/
}
