using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Text;

namespace BTLAB11
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public int Gold { get; set; }
        public int Coins { get; set; }
        public bool IsActive { get; set; }
        public int VipLevel { get; set; }
        public string Region { get; set; } = "";
        public DateTime LastLogin { get; set; }
    }

    
    public class Player02
    {
        public string Name { get; set; } = "";
        public int Gold { get; set; }
        public int Coins { get; set; }
    }

    class Program
    {
        private static FirebaseClient firebase = new FirebaseClient("https://thevy32-17d34-default-rtdb.asia-southeast1.firebasedatabase.app/");
        

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;


            var httpClient = new HttpClient();
            string url = "https://raw.githubusercontent.com/NTH-VTC/OnlineDemoC-/main/simple_players.json";      
            string json = await httpClient.GetStringAsync(url);


            var players = JsonConvert.DeserializeObject<List<Player>>(json);

           
            var richPlayers = players
                .Where(p => p.Gold > 1000 && p.Coins > 100)
                .OrderByDescending(p => p.Gold)
                .Select(p => new Player02 { Name = p.Name, Gold = p.Gold, Coins = p.Coins })
                .ToList();

            Console.WriteLine("Player có Gold > 1000 và Coins > 100:");
            foreach (var p in richPlayers)
            {
                Console.WriteLine($"Tên: {p.Name}, Gold: {p.Gold}, Coins: {p.Coins}");
            }

            
            await firebase
                .Child("quiz_bai1_richPlayers")
                .PutAsync(richPlayers);

           
        }
    }
}