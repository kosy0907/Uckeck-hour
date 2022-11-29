using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

public class ScoreData : MonoBehaviour
{
    public GameObject score;
    FirebaseFirestore db;
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        CollectionReference usersRef = db.Collection("ranking");
        usersRef.OrderByDescending("time").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Dictionary<string, object> documentDictionary = document.ToDictionary();
	   
                score.GetComponent<UnityEngine.UI.Text>().text = score.GetComponent<UnityEngine.UI.Text>().text+string.Format("Nickname: {0}, Time: {1}\n", documentDictionary["nickname"], documentDictionary["time"]);
                
            }
            Debug.Log("Read all data from the users collection.");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
