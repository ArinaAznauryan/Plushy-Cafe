using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;
using PixelCrushers.DialogueSystem;
using UnityEngine.Events;
using PixelCrushers;
using UnityEngine.AI;

    public class PolarCord : Coordinates {
        public float radius, angle;

        public PolarCord(float radius, float angle) {
            this.radius = radius;
            this.angle = angle;
        }
    }

    public class KartesianCord : Coordinates {
        public float x, y;

        public KartesianCord(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }

    public class Coordinates {
        public KartesianCord PolarToKartesian2D(PolarCord polar) {
            return new KartesianCord(polar.radius * Mathf.Cos(polar.angle), polar.radius * Mathf.Sin(polar.angle));
        }

        public PolarCord KartesianToPolar2D(KartesianCord kartesian) {
            return new PolarCord(Mathf.Sqrt(kartesian.x*kartesian.x + kartesian.y*kartesian.y), Mathf.Atan(kartesian.y/kartesian.x));
        }
    }

    [System.Serializable]
    public class Items {
        public List<string> itemNames, foodNames, allergyNames;
        public List<ScratchFood> foodsFromScratch;
        public List<string> foodsPremade;
        public FoodsWithAllergies[] foodsWithAllergies;
        public List<string> blenderFood;
        public List<string> ingredients;
        public List<List<List<string>>> kitchenToolsCorFoods;
    }

    public class KitchenToolsCorFoods
    {
        public List<string> blender { get; set; }
        public List<string> pan { get; set; }
    }

    [System.Serializable]
    public class ScratchFood {
        public string name;
        public List<string> ingredients;
    }

    [System.Serializable]
    public class FoodsWithAllergies {
        public string name;
        public List<string> allergies;
    }

    [System.Serializable]
    public class AnimalList
    {
        public List<string> animals;
    }

    public class MyTools : MonoBehaviour
    {
        public Camera playerCamera;
        TextAsset itemsJson, animalsJson;
        Items allItemsJson;
        List<string> itemNames;
        List<string> foodNames;
        List<string> allergyNames;
        List<FoodsWithAllergies> foodsWithAllergies;
        List<string> blenderFood;

        List<string> animalNames;

        List<AudioClip> soundEffects;

        private GameObject objOnFocus;
        private GameObject defaultAnimalPrefab;

        public MyPlayer player;

        float cosAngle = 0;

        public void Awake() {
            //playerCamera = GameEventsManager.instance.player.transform.GetComponentsInChildren<Camera>()[0];
            LoadItemData();
            LoadAnimalData();
            LoadMusicData();
            defaultAnimalPrefab = Resources.Load("defaultAnimalPrefab") as GameObject;

            SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        void Start() {
            player = GameEventsManager.instance.player;
        }

        public void PlaySound(AudioSource audio, string name) {
            if (name is "") audio.clip = null;
            else audio.clip = soundEffects.Where(x => x.name.Equals(name)).SingleOrDefault();
            Debug.Log("Name:" + audio.clip.name);
            audio.Play();
        }

        public void StopSound(AudioSource audio) {audio.Stop();}
    

    private void ChangedActiveScene(Scene current, Scene next)
    {
        Debug.Log("Scene names: " + current.name + ", " + next.name);

        if (/*current.name == "StartScene" && */next.name == "Playground")
        {
            Debug.Log("Changed to playground");
            
        }
    }

    

        void LoadMusicData() {
            soundEffects = Resources.LoadAll<AudioClip>("SoundEffects").ToList();
        }
        

        void LoadItemData() {
            itemsJson = Resources.Load<TextAsset>("ItemsData/AllItems");
            //allItemsJson = JsonUtility.FromJson<Items>(itemsJson.text);
            allItemsJson = JsonConvert.DeserializeObject<Items>(itemsJson.ToString());
            itemNames = allItemsJson.itemNames.ToList();
            foodNames = allItemsJson.foodNames.ToList();
            allergyNames = allItemsJson.allergyNames.ToList();
            foodsWithAllergies = allItemsJson.foodsWithAllergies.ToList();
            blenderFood = allItemsJson.blenderFood.ToList();
        }

        void LoadAnimalData() {
            AnimalList allAnimals = JsonUtility.FromJson<AnimalList>( Resources.Load<TextAsset>("AnimalsData/allAnimals").text );
            animalNames = allAnimals.animals.ToList();
        }

        public void FixedUpdate() {
            UpdateObjectFocus();
        }

        public void GenerateRandomAnimal() {
            //Debug.Log("Did u happen?");
            GameObject randomAnimal = Instantiate(defaultAnimalPrefab, new Vector3(-132.42f, 2.45f, -49.96f), Quaternion.identity);
            string animalPath = "Models/AnimalModels/" + animalNames[UnityEngine.Random.Range(0, animalNames.Count-1)];
            GameObject randomAnimalMesh = Resources.Load(animalPath) as GameObject;
            //randomAnimal.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh = randomAnimalMesh.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        }

        public bool PlayerInCafe() {
            InsideOrOutside border = FindFirstObjectByType<InsideOrOutside>();
            
            return (border.transform.position.x - player.transform.position.x < 0f);
        }

        public bool IsEqualOnRandOrder(List<string> curList, List<string> tarList) {
            List<string> bufList = tarList;
            int tarListCount = tarList.Count;
            int factor = 0;
            curList = curList.Distinct().ToList();
            
            if ((curList.Distinct().ToList()).Count == tarList.Count) {
                foreach (string value in (curList.Distinct().ToList())) {

                    if (bufList.Any(x => x.Equals(value))) {
                        factor++;
                        bufList.Remove(value);
                    }
                }

                if (factor == tarListCount) {
                    return true;
                }

                return false; 
            }
            return false;
        }

        string DelSubString(string word, string from) {
            int index = word.IndexOf(from);
            string cleanPath = word.Remove(0, index+from.Length);

            Debug.Log("Result:" + cleanPath);
            return cleanPath;
        }

        public GameObject LoadPrefabOnRandOrderPath(List<string> values, string path) {

            //DirectoryInfo dir = new DirectoryInfo("Assets/Resources/"+path);
            //FileInfo[] info = dir.GetFiles("*.prefab");
            List<GameObject> objects = (Resources.LoadAll<GameObject>(path)).ToList();
            foreach (GameObject obj in objects) 
            {
                Debug.Log("file name: " + obj.name.ToString());
                foreach (string name in ((obj.name).ToString()).Split(' ').ToList()) {
                    Debug.Log("Name: " + name);
                }
               // Debug.Log("AUR NO:" + DelSubString(obj.ToString(), "Resources"));
                //GameObject test = (GameObject)Resources.Load<GameObject>(DelSubString(f.ToString(), "Resources"));
               // Debug.Log ("Found: " + test);
                
                if (IsEqualOnRandOrder(values, ((obj.name).ToString()).Split(' ').ToList())) {
                    Debug.Log("FOUND IT");
                    return obj;
                }
            }
            return null;
        }

        public Dish DefineDish(List<string> ingredients) {
            //new Dish(new Food("defaultFood", 1))
            //public ScratchFood[] foodsFromScratch;
            //string[] ingredients;
            List<ScratchFood> allScratchIngredients = allItemsJson.foodsFromScratch.ToList();
            //foodsWithAllergies.Where(x => x.name.Equals(food.name)).ToList();
            //List<string> allIngredients = allItemsJson.ingredients.ToList();

            foreach (ScratchFood ingredientGroup in allScratchIngredients){
                if (IsEqualOnRandOrder(ingredients, ingredientGroup.ingredients.ToList())) {
                    Food resultFood = new Food(ingredientGroup.name, 1);
                    return new Dish(resultFood, GetFoodAllergies(resultFood));
                }
            }
            Debug.LogError("Couldn't define a dish!");
            return null;
        }

        public void CosWave(Transform tar, float amplitude, float speed) {
            cosAngle += speed;
            tar.transform.position += new Vector3(0f, Mathf.Cos(cosAngle)*amplitude*Time.deltaTime, 0f);
            if (cosAngle > 360f) cosAngle = 0;
        }

        public void EnableQuestionMark(Transform tarTransform) {
            GameObject questionMark = Resources.Load<GameObject>("UI/questionMark");
            questionMark = Instantiate(questionMark, new Vector3(tarTransform.position.x, tarTransform.position.y+3f, tarTransform.position.z), Quaternion.identity);
            questionMark.transform.parent = tarTransform;
            questionMark.name = "questionMark";

            GameEventsManager.instance.Updatables.AddUpdatable(() => questionMarkLook(questionMark));
        }

        // public GameObject InstantiateWithNetwork(GameObject prefab, Vector3 position) {
        //     GameObject result = Instantiate(prefab, position, Quaternion.identity);
        //     GlobalManager.Instance.SpawnGlobalObject(result);
        //     // List<Transform> childTransforms = new List<Transform>();

        //     // foreach (Transform child in result.transform) {
        //     //     childTransforms.Add(child);
        //     // }

        //     // childTransforms.RemoveAt(0);
        //     // // GameObject savedReference;
        //     // // savedReference.transform.parent = result.parent;
        //     // // savedReference.transform.localPosition = new Vector3(0, 0, 0);

        //     // for (int i = 0; i < result.transform.childCount; i++) {
        //     //     GameObject child = result.transform.GetChild(i).gameObject;
        //     //     if (child.GetComponent<NetworkObject>()) 
        //     //     {
        //     //         child.transform.parent = result.transform;
        //     //         AssignTransform(child.transform, childTransforms[i]);

        //     //         GlobalManager.Instance.SpawnGlobalObject(child);
        
        //     //     }
        //     // }
            
        //     //Destroy(savedReference);

        //     return result;
        // }

        public void AssignTransform(Transform target, Transform target2) {
            target.position = target2.position;
            target.rotation = target2.rotation;
            target.localScale = target2.localScale;
        }

        public void DisableQuestionMark(Transform tarTransform) {
            GameObject questionMark = tarTransform.Find("questionMark").gameObject;
            Destroy(questionMark);
        }

        void questionMarkLook(GameObject questionMark = null) {
            if (questionMark) {
                questionMark.transform.LookAt(questionMark.transform.position + playerCamera.transform.rotation * Vector3.forward,
                playerCamera.transform.rotation * Vector3.up);

                CosWave(questionMark.transform, 1f, 0.2f);
            }
        }

        public Type GetItemType(string name) {
            if (itemNames.Any(x => x.Equals(name))) return typeof(KitchenTool);
            if (foodNames.Any(x => x.Equals(name))) return typeof(Dish);
            Debug.LogError("Item is invalid for type identification!");
            return null;
        }

        public GameObject GetPrefab(string path) {
            return Resources.Load(path) as GameObject;
        }

        public void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++) arr[a] = arr[a + 1];
            System.Array.Resize(ref arr, arr.Length - 1);
        }

        public void CheckObjectOnFocus()
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10))
            {
                objOnFocus = hit.collider.gameObject;
            }
            else objOnFocus = null;
        }

        public GameObject ObjectOnFocus() {
            Usable objUsable = objOnFocus?.GetComponent<Usable>();
            if (objUsable && objUsable.enabled) return objOnFocus;
            return null;
        }


        public Transform FindMeshChild(Transform parent) {
            foreach (Transform child in parent) {
                if (child.gameObject.GetComponent<MeshRenderer>() != null) return child;
            }
            return null;
        } 

        public void UpdateObjectFocus() {
            CheckObjectOnFocus();
        }

        public List<Allergy> GetFoodAllergies(Food food) {
            List<FoodsWithAllergies> result = foodsWithAllergies.Where(x => x.name.Equals(food.name)).ToList();
            
            if (result.Count < 1) return new List<Allergy> { new Allergy("defaultAllergy"), new Allergy("defaultAllergy") };

            List<Allergy> allergies = new List<Allergy>();
            foreach (string str in result[0].allergies) {
                allergies.Add(new Allergy(str));
            }

            if (allergies.Count >  0) return allergies;
            return new List<Allergy>{new Allergy("defaultAllergy"), new Allergy("defaultAllergy")};
        }

        public bool IfCorrespondingFood(InventoryItem tool, Food food) {
            KitchenTool kitchenTool = tool as KitchenTool;
            for (int i = 0; i < allItemsJson.kitchenToolsCorFoods.Count; i++) {
                var items = allItemsJson.kitchenToolsCorFoods;
                if (kitchenTool.name == items[i][0][0] && items[i][1].Contains(food.name)) {
                    return true;
                }
            }
            return false;
        }

        public AnimalOrder GenerateRandomAnimalOrder() {
            List<Dish> randDishes = PickSubListOnRandom(foodNames, new int[] {4, 8}, new Dish(new Food("defaultFood", 1), new List<Allergy>()));
            List<Allergy> randAllergies = PickSubListOnRandom(allergyNames, new int[] {2, 6}, new Allergy("defaultAllergy"));
            string taskID = "BluePigTask";
            Order randOrder = new Order(randDishes, randAllergies);
 
            return new AnimalOrder("unassigned", 120, randOrder, taskID);
        }

        List<string> PickSubListOnRandom(List<string> tarList, int[] priority) {
            List<string> newFoods = new List<string>();
            int foodAmount = GetRandomValue(priority);
            
            for (int i = 0; i <= foodAmount; i++) {
                newFoods.Add(tarList[GetRandomValue(null, new int[] {0, tarList.Count-1})]);
            }

            return newFoods;
        }

        List<Dish> PickSubListOnRandom(List<string> tarList, int[] priority, Dish dish) {
            List<Dish> newDishes = new List<Dish>();
            int dishAmount = GetRandomValue(priority);
            
            for (int i = 0; i <= dishAmount; i++) {
                string randFoodName = tarList[UnityEngine.Random.Range(0, tarList.Count-1)];
                Food randFood = new Food(randFoodName, 1);
                newDishes.Add( new Dish( randFood, GetFoodAllergies(randFood) ) );
            }

            return newDishes.Distinct(new DishComparer()).ToList();
        }

        List<Allergy> PickSubListOnRandom(List<string> tarList, int[] priority, Allergy allergy) {
            List<Allergy> newAllergies = new List<Allergy>();
            int allergyAmount = GetRandomValue(priority);
            
            for (int i = 0; i <= allergyAmount; i++) {
                newAllergies.Add(
                    new Allergy(tarList[UnityEngine.Random.Range(0, tarList.Count-1)])
                );
            }

            return newAllergies.Distinct(new AllergyComparer()).ToList();
        }

        int GetRandomValue(int[] priority, int[] range = null) {

            if (priority == null) return UnityEngine.Random.Range(range[0], range[1]);

            float rand = UnityEngine.Random.value;
            if (rand <= .5f)
                return UnityEngine.Random.Range(0, priority[0]);
            if (rand <= .8f)
                return UnityEngine.Random.Range(priority[0]+1, priority[0]+3);
    
            return UnityEngine.Random.Range(priority[1], priority[1]+2);
        }

        public Vector2 direction(Vector2 position1, Vector2 position2) {
            var dir = (position1 - position2).normalized;
            if (dir.x > 0 & dir.y > 0) dir = new Vector2(1f, 1f);
            else if (dir.x < 0 & dir.y < 0) dir = new Vector2(-1f, -1f);
            else if (dir.x < 0 & dir.y > 0) dir = new Vector2(-1f, 1f);
            else if (dir.x > 0 & dir.y < 0) dir = new Vector2(1f, -1f);

            else if (dir.x == 0 && dir.y != 0) {
                if (dir.y < 0) new Vector2(0f, -1f);
                else new Vector2(0f, 1f);
            }

            else if (dir.y == 0 && dir.x != 0) {
                if (dir.x < 0) dir = new Vector2(-1f, 0f);
                else dir = new Vector2(1f, 0f);
            }

            return dir;
        }

        public float normalizeDirect(Vector2 pos, Vector2 pos1) {
            return (pos - pos1).normalized.x > 0 ? 1 : -1;
        }


        public int GetClosestObjectIndex<T>(ref List<T> array, GameObject target) where T : MonoBehaviour {
            List<float> distances = new List<float>();
            int closestObject = 0;

            for (int i = 0; i < array.Count; i++) {
                distances.Add(Vector2.Distance(target.transform.position, array[i].gameObject.transform.position));
                float minVal = distances.Min();
                closestObject = distances.IndexOf(minVal);

                if (i==(array.Count-1)) break;
                
            }

            return closestObject;
        }


        public IEnumerator animFinished(float delay, System.Action<bool> callback) {
            yield return new WaitForSeconds(delay);
            callback(true);
        }

        public List<GameObject> GetGameObjectInChildren(GameObject parent, GameObject excludedTarget = null) {
            List<GameObject> list = new List<Transform>(parent.GetComponentsInChildren<Transform>()).ConvertAll<GameObject>(delegate(Transform p_it) { return p_it.gameObject; });
            list.RemoveAt(0);
            if (excludedTarget) list.Remove(excludedTarget);
            return list;
        }

       
        public GameObject FindObject(GameObject parent, string name) {
            Transform[] trs= parent.GetComponentsInChildren<Transform>(true);
            foreach(Transform t in trs){
                if(t.name == name) return t.gameObject;
            }
            return null;
        }

        public bool ContainsIngredients() {
            return true;
        }

        public void GoTo(Transform tar, Transform destination) {
            MyAgent agent = new MyAgent(tar.GetComponent<NavMeshAgent>());

            agent.GoTo(destination);

            if (tar.GetComponent<AnimatorAgentSync>() is null) {
                tar.gameObject.AddComponent<AnimatorAgentSync>();
                tar.GetComponent<AnimatorAgentSync>().LinkAnimatorToAgent(agent);
            }
        }
    }

public class AnimalGuest {
    public AnimalOrderTrigger animal;
}

public class AnimalGuestGroup {
    public AnimalOrderTrigger animal;
    public List<GameObject> npcs;

    public AnimalGuestGroup(AnimalOrderTrigger animal, List<GameObject> npcs) {
        this.animal = animal;
        this.npcs = npcs;
    }
}


class DishComparer : IEqualityComparer<Dish>
{
    public bool Equals(Dish x, Dish y) { if (x.food.name == y.food.name) return true; else return false;}

    public int GetHashCode(Dish dish) { return 0; }
}

class AllergyComparer : IEqualityComparer<Allergy>
{
    public bool Equals(Allergy x, Allergy y) { if (x.name == y.name) return true; else return false;}

    public int GetHashCode(Allergy allergy) { return 0; }
}

public class QuestionMark {
    public bool isEnabled = false;
}

public class MyAgent {
    public NavMeshAgent agent;
    bool jumped = false;
    Transform destination;

    public MyAgent(NavMeshAgent agent) {
        Debug.Log("Speed: " + agent.angularSpeed);
        this.agent = agent;
    }

    public void GoTo(Transform destination) {
        agent.SetDestination(destination.position);
        this.destination = destination;
        agent.angularSpeed = 120;
    }

    public bool OnAgentJump() {
        jumped = true;
        return agent.isOnOffMeshLink;
    }

    void checkIfJumped() {
        if (jumped) jumped = OnAgentIdle();
    }

    public bool OnAgentMove() {
        return !OnAgentReachDestination();
    }

    public bool OnAgentIdle() {
        return OnAgentReachDestination();
    }

    public bool OnAgentHighIdle() {
        checkIfJumped();
        return jumped;
    }

    public void FaceDestinationTarget() {
        if (destination) {
            agent.angularSpeed = 0;
            //float yValue = Mathf.Abs(destination.localRotation.y) == 90f ? destination.localRotation.y + 180f : destination.localRotation.y - 180f;
            agent.transform.rotation = new Quaternion(agent.transform.rotation.x, destination.localRotation.y, agent.transform.rotation.z, agent.transform.rotation.w);
        }
    }

    public bool OnAgentReachDestination() {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (destination) {
                        FaceDestinationTarget();
                        agent.isStopped = true;
                        agent.ResetPath();
                    }

                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }
}