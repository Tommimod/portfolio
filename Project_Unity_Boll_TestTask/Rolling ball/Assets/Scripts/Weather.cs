using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public partial class Weather : MonoBehaviour {
    public Text First;
    public Text Two;
    public Text City;
    public Text Cels;
    public Text Weat;
    private string[] citys = {"London", "Berlin", "Brussels", "Moscow", "Tokyo", "Vienna",
    "Warsaw", "Lisbon", "Kiev"};

   public IEnumerator Start_cor()
    {
        var i = Random.Range(0, citys.Length);

        string url = "http://api.openweathermap.org/data/2.5/weather?q="+citys[i]+"&units=metric&appid=16596e354328ba2e3ce1ed54b3fbe1ad";

        WWW request = new WWW(url);
        yield return request;

        if (request.error == null || request.error == "")
        {
            JSONObject tempData = new JSONObject(request.text);
            //get the data as a JSON Object and then get the first value of Main
            JSONObject nameDetails = tempData["name"];
            string nameType = nameDetails.str;

            JSONObject tempDetails = tempData["main"];
            float tempType = tempDetails["temp"].n;

            First.text = nameType + " " + tempType.ToString() + "C";

            JSONObject weatherDetails = tempData["weather"];
            string weatherType = weatherDetails[0]["main"].str;

            Two.text = weatherType;

            City.text = "City: " + nameType;
            Cels.text = tempType.ToString() + "C";
            Weat.text = weatherType;
}
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }

    private void Start()
    {
        StartCoroutine("Start_cor");
    }
}
