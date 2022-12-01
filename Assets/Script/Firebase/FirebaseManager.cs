using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine;

class ClearDescription{
  public string nickname {get; set;}
  public double clearTime {get; set;}
}

public class FirebaseManager : MonoBehaviour
{
  FirebaseFirestore db;

  void Start()
  {
      db = FirebaseFirestore.DefaultInstance;
      WriteData("1","steven",1.1); //문제번호, 사용자명, 시간(분)
      ReadData("1");
  }

  public void WriteData(string missionNum, string nickname, double clearTime)
  {
      Debug.Log("Firebase Add Data.");
      DocumentReference docRef = db.Collection(missionNum).Document();
      Dictionary<string, object> user = new Dictionary<string, object>
      {
              { "Nickname", nickname },
              { "Cleartime", clearTime },
      };
      docRef.SetAsync(user).ContinueWithOnMainThread(task => {
              Debug.Log("Added data to the alovelace document in the users collection.");
      });
  }

  public void ReadData(string missionNum)
  {
      Debug.Log("Firebase Read Data.");
      CollectionReference usersRef = db.Collection(missionNum);
      usersRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
      {
          QuerySnapshot snapshot = task.Result;
          foreach (DocumentSnapshot document in snapshot.Documents)
          {
              Dictionary<string, object> documentDictionary = document.ToDictionary();
              Debug.Log(string.Format("Nickname: {0}", documentDictionary["Nickname"]));
              Debug.Log(string.Format("Cleartime: {0}", documentDictionary["Cleartime"]));

          }

          Debug.Log("Read all data from the users collection.");
      });
  }
}