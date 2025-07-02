using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Text;

namespace BTLAB1102
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

    public class VipPlayer
    {
        public string Name { get; set; } = "";
        public int VipLevel { get; set; }
        public DateTime LastLogin { get; set; }
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

            
            int totalVip = players.Count(p => p.VipLevel > 0);
            Console.WriteLine($"Tổng số người chơi VIP: {totalVip}");

            
            var vipByRegion = players
                .Where(p => p.VipLevel > 0)
                .GroupBy(p => p.Region)
                .Select(g => new { Region = g.Key, Count = g.Count() });

            Console.WriteLine("\n Số lượng người chơi VIP theo khu vực:");
            foreach (var group in vipByRegion)
            {
                Console.WriteLine($"- Khu vực: {group.Region}, Số người chơi VIP: {group.Count}");
            }

            
            DateTime now = new DateTime(2025, 06, 30, 0, 0, 0); 

            var recentVipPlayers = players
                .Where(p => p.VipLevel > 0 && (now - p.LastLogin).TotalDays <= 2)
                .Select(p => new VipPlayer
                {
                    Name = p.Name,
                    VipLevel = p.VipLevel,
                    LastLogin = p.LastLogin
                })
                .ToList();

            Console.WriteLine("\nNgười chơi VIP mới đăng nhập:");
            foreach (var p in recentVipPlayers)
            {
                Console.WriteLine($"Tên: {p.Name}, VIP: {p.VipLevel}, Đăng nhập: {p.LastLogin}");
            }

            
            await firebase
                .Child("quiz_bai2_recentVipPlayers")
                .PutAsync(recentVipPlayers);          
        }
    }
}