using System;
using System.Collections.Generic;
using static ConsoleApp2.Program;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp2
{
    internal class Program
    {
        //메인 함수
        static void Main(string[] args)
        {
            NewStart();
        }

        interface IMyInterface
        {
            void DisplayStats();
            void DisplayInventory();
        }

        //게임 시작 & 저장
        static void NewStart()
        {
            GameDataManager savefile = new GameDataManager();

            Player loadedPlayer = savefile.LoadPlayerData();

            if (loadedPlayer != null)
            {
                Console.WriteLine($"환영합니다 {loadedPlayer.name}님! 이전 저장 데이터를 불러왔습니다.");
                Home(loadedPlayer);
            }
            else
            {
                Console.WriteLine("혼돈의 이세계오신걸 환영합니다\n");
                Console.Write("플레이어의 이름을 적어주세요: ");
                string username = Console.ReadLine();
                Player player = new Player
                {
                    name = username,
                    level = 1,
                    gold = 1000,
                    health = 100,
                    akp = 10,
                    dkp = 5,
                    exp = 0,
                    inventory = new List<Item>()
                };

                savefile.SavePlayerData(player);

                Console.WriteLine($"반갑습니다 {username}님! 마을로 이동하여 새로운 모험을 시작해주세요!\n");
                Home(player);
            }
        }

        //마을 
        static void Home(Player player)
        {
            Console.WriteLine("마을에 도착했습니다. 무엇을 할까요? \n");
            Console.WriteLine("1. 여관 | 골드를 지불하고 피곤함을 날린다.");
            Console.WriteLine("2. 상점 | 새로운 장비를 구입, 판매할 수 있습니다.");
            Console.WriteLine("3. 여행 | 알 수 없는 미지의 세계로 여행을 떠납니다.");
            Console.WriteLine("4. 내 상태 확인");
            Console.WriteLine("9. 게임 저장\n");

            Console.Write("원하시는 행동을 입력해주세요.\n>>");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            GoHotel(player);
                            break;
                        case 2:
                            GoShop(player);
                            break;
                        case 3:
                            GoDungeon(player);
                            break;
                        case 4:
                            CheckMyState(player); 
                            break;
                        case 9:
                            GameDataManager savefile = new GameDataManager();
                            savefile.SavePlayerData(player);
                            return;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            break;
                    }
                }
            }
        }

        //내 상태확인
        public static void CheckMyState(Player player) // Player 객체를 인자로 받음
        {
            Console.Clear();
            Console.WriteLine("[내 상태 확인]");
            Console.WriteLine("1.스탯확인");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3.장비장착");
            Console.WriteLine("9.뒤로");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Clear();
                            player.DisplayStats(); // Player 스탯 출력
                            break;
                        case 2:
                            Console.Clear();
                            player.DisplayInventory(); 
                            break;
                        case 3:
                            Console.Clear();
                            player.Equipment();
                            break;
                        case 9:
                            Home(player);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            break;
                    }
                }
            }
        }

        //여관
        public static void GoHotel(Player player)
        {
            Console.Clear();
            Console.WriteLine("포근한 여관에 도착했습니다.");
            Console.WriteLine("1. 여관이용 | 50Gold - 체력이 회복됩니다.");
            Console.WriteLine("9. 돌아가기");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            bool success = player.RemoveGold(50);
                            player.health = 100;
                            break;
                        case 9:
                            Home(player);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            break;
                    }
                }
            }
        }
        
        //상점
        public static void GoShop(Player player)
        {
            Console.Clear();
            Console.WriteLine("여러 장비가 진열된 상점에 들어왔습니다.\n");
            Console.WriteLine("보유 골드");
            Console.WriteLine("["+player.gold +"G]\n" );
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("- 1 수련자 갑옷    | 방어력 +5  | 수련에 도움을 주는 갑옷입니다.      |  300 G   ");
            Console.WriteLine("- 2 무쇠갑옷       | 방어력 +9  | 무쇠로 만들어져 튼튼한 갑옷입니다.  |  1200 G");
            Console.WriteLine("- 3 스파르타의 갑옷 | 방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다.|  3500 G");
            Console.WriteLine("- 4 가시 갑옷      | 방어력 +55 |      15%확률로 받는 데미지로 공격합니다.    |  10,500 G");
            Console.WriteLine("- 5 낡은 검        | 공격력 +2  | 쉽게 볼 수 있는 낡은 검 입니다.               |  600 G");
            Console.WriteLine("- 6 청동 도끼      | 공격력 +5  |  어디선가 사용됐던거 같은 도끼입니다.          |  1500 G");
            Console.WriteLine("- 7 스파르타의 창  | 공격력 +7  | 스파르타의 전사들이 사용했다는 전설의 창입니다. |  4000 G\n");
            Console.WriteLine("- 8 무한의 대검    | 공격력 +30  | 치명타 확률 30%+ / 치명타 공격력 25%+        | 28,000 G");
            Console.WriteLine("0. 판매하기");
            Console.WriteLine("9. 돌아가기");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            if (player.RemoveGold(300)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("수련자 갑옷", ItemType.Armor, 5, 300)); // 아이템 추가
                                Console.WriteLine("수련자 갑옷을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 2:
                            if (player.RemoveGold(1200)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("무쇠갑옷", ItemType.Armor, 9, 1200)); // 아이템 추가
                                Console.WriteLine("무쇠갑옷을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 3:
                            if (player.RemoveGold(3500)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("스파르타의 갑옷", ItemType.Armor, 15, 3500)); // 아이템 추가
                                Console.WriteLine("스파르타의 갑옷을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 4:
                            if (player.RemoveGold(10500)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("가시 갑옷", ItemType.Armor, 2, 10500)); // 아이템 추가
                                Console.WriteLine("낡은 검을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 5:
                            if (player.RemoveGold(600)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("낡은 검 ", ItemType.Weapon, 2, 600)); // 아이템 추가
                                Console.WriteLine("청동 도끼를 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 6:
                            if (player.RemoveGold(1500)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("청동 도끼 ", ItemType.Weapon, 5, 1500)); // 아이템 추가
                                Console.WriteLine("스파르타의 창을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 7:
                            if (player.RemoveGold(4000)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("스파르타의 창 ", ItemType.Weapon, 7, 4000)); // 아이템 추가
                                Console.WriteLine("스파르타의 창을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 8:
                            if (player.RemoveGold(28000)) // 골드 제거 성공
                            {
                                player.inventory.Add(new Item("무한의 대검 ", ItemType.Weapon, 30, 28000)); // 아이템 추가
                                Console.WriteLine("스파르타의 창을 구매했습니다!");
                            }
                            else
                            {
                                Console.WriteLine("골드가 부족하여 구매할 수 없습니다.");
                            }
                            break;
                        case 0:
                            SellShop(player);
                            break;
                        case 9:
                            Home(player);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            break;
                    }
                }
            }
        }

        //던전
        public static void GoDungeon(Player player)
        {
            Console.Clear();
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("9. 돌아가기 \n");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            EnterDungeon(player, 5, 1000, 5); 
                            break;
                        case 2:
                            EnterDungeon(player, 11, 1700, 35); 
                            break;
                        case 3:
                            EnterDungeon(player, 17, 2500, 80); 
                            break;
                        case 9:
                            Program.Home(player);
                            break;
                        default:
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            break;
                    }
                }
            }
        }
        
        //판매하기
        public static void SellShop(Player player)
        {
            while (true) 
            {
                Console.WriteLine("판매하실 아이템의 번호를 선택해주세요.\n");
                player.DisplayInventory();
                Console.WriteLine("9. 돌아가기\n"); 

                if (int.TryParse(Console.ReadLine(), out int itemChoice))
                {
                    if (itemChoice == 9)
                    {
                        GoShop(player);
                        break;
                    }

                    if (itemChoice > 0 && itemChoice <= player.inventory.Count)
                    {
                        Item itemToSell = player.inventory[itemChoice - 1];
                        int sellingPrice = (int)(itemToSell.value * 0.8);

                        player.AddGold(sellingPrice);
                        player.inventory.RemoveAt(itemChoice - 1); 

                        Console.WriteLine($"{itemToSell.name}을 판매했습니다. {sellingPrice} G가 추가되었습니다.\n");
                        player.DisplayInventory();
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 유효한 아이템 번호를 입력해주세요.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
                }
            }
        }

        //던전진행
        private static void EnterDungeon(Player player, int recommendedDefense, int baseReward, int baseexp)
        {
            Random random = new Random();
            if (player.dkp < recommendedDefense)
            {
                if (random.NextDouble() <= 0.4)
                {
                    Console.WriteLine("던전 탐험에 실패했습니다!");
                    player.health /= 2;
                    Console.WriteLine($"체력이 반으로 감소했습니다. 현재 체력: {player.health}");
                    return;
                }
                else
                {
                    Console.WriteLine("던전 탐험에 성공했습니다! 보상 획득이 가능합니다.");
                }
            }
            else
            {
                int damage = random.Next(20, 36) - (player.dkp - recommendedDefense);
                if (damage < 0) damage = 0; 
                player.health -= damage;
                Console.WriteLine($"던전 탐험을 마쳤습니다. 체력이 {damage}만큼 감소했습니다. 현재 체력: {player.health}");
            }

            int extraRewardPercentage = random.Next(0, 101); 
            int additionalReward = (int)(baseReward * (player.akp / (float)(player.akp * 2)));
            int totalReward = baseReward + additionalReward;

            Console.WriteLine($"기본 보상: {baseReward} G + 추가 보상: {additionalReward} G = 총 보상: {totalReward} G\n");
            player.AddGold(totalReward);
            player.AddExp(baseexp);

            Console.WriteLine("여행을 이어가겠습니까? \n");
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("9. 마을로 돌아가기 \n");
        }

        //플레이어 클레스
        public class Player : IMyInterface
        {
            public Item CurrentWeapon { get; set; }
            public Item CurrentArmor { get; set; }
            public string name { get; set; }
            public int level { get; set; }
            public int gold { get; set; } // 소지 골드
            public int health { get; set; } // 체력
            public int akp { get; set; } // 공격력 
            public int dkp { get; set; } // 방어력
            public int exp { get; set; } // 현재 경험치
            public List<Item> inventory { get; set; }

            public void EquipWeapon(Item weapon)
            {
                if (weapon.type == ItemType.Weapon)
                {
                    CurrentWeapon = weapon;
                    Console.WriteLine($"{weapon.name}을(를) 장착했습니다.");
                }
                else
                {
                    Console.WriteLine($"{weapon.name}은(는) 무기가 아닙니다.");
                }
            }
            public void EquipArmor(Item armor)
            {
                if (armor.type == ItemType.Armor)
                {
                    CurrentArmor = armor;
                    Console.WriteLine($"{armor.name}을(를) 장착했습니다.");
                }
                else
                {
                    Console.WriteLine($"{armor.name}은(는) 갑옷이 아닙니다.");
                }
            }
            
            public void AddGold(int amount)
            {
                if (amount > 0)
                {
                    gold += amount;
                    Console.WriteLine($"{amount} G가 추가되었습니다. 현재 골드: {gold} G");
                }
            }
            public bool RemoveGold(int amount)
            {
                if (amount > 0 && gold >= amount)
                {
                    gold -= amount;
                    Console.WriteLine($"{amount} G가 차감되었습니다. 현재 골드: {gold} G");
                    return true; // 성공적으로 제거
                }
                else if (amount <= 0)
                {
                    Console.WriteLine("제거할 골드는 양수여야 합니다.");
                }
                else
                {
                    Console.WriteLine("골드가 부족합니다.");
                }
                return false; // 실패
            }

            public void DisplayStats() // 내 정보 출력
            {
                Console.WriteLine("=== 플레이어 정보 ===");
                Console.WriteLine($"이름: {name}");
                Console.WriteLine($"레벨: {level}");
                Console.WriteLine($"소지 골드: {gold} G");
                Console.WriteLine($"체력: {health}");
                if (CurrentWeapon != null)
                {
                    Console.WriteLine($"공격력: {akp} + [{CurrentWeapon.power}] (장비: {CurrentWeapon.name})");
                }
                else
                {
                    Console.WriteLine($"공격력: {akp}"); 
                }

                if (CurrentArmor != null)
                {
                    Console.WriteLine($"방어력: {dkp} + [{CurrentArmor.power}] (장비: {CurrentArmor.name})");
                }
                else
                {
                    Console.WriteLine($"방어력: {dkp}"); 
                }
                Console.WriteLine($"경험치: {exp}");
                Console.WriteLine("=====================");
                Console.WriteLine("9. 돌아가기");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        if (choice == 9)
                        {
                            Program.Home(this);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                        }
                    }
                }
            }

            public void DisplayInventory()
            {
                if (inventory == null || inventory.Count == 0)
                {
                    Console.WriteLine("인벤토리가 비어있습니다.");
                }
                else
                {
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        Item item = inventory[i];
                        string equipped = "";

                        // 무기와 방어구 장착 상태 확인
                        if (item == CurrentWeapon)
                        {
                            equipped = "[E]"; // 무기 장착 표시
                        }
                        else if (item == CurrentArmor)
                        {
                            equipped = "[E]"; // 방어구 장착 표시
                        }

                        Console.WriteLine($"{i + 1}. {equipped}{item.name} | 타입: {item.type} | 공격력/방어력: {item.power}");
                    }
                    Console.WriteLine("9. 돌아가기");
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out int choice))
                        {
                            if (choice == 9)
                            {
                                Program.Home(this);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                            }
                        }
                    }
                }
               
            }

            public void Equipment()
            {
                while (true)
                {
                    Console.WriteLine("\n[장비 관리] 보유 중인 아이템 목록:");
                    if (inventory == null || inventory.Count == 0)
                    {
                        Console.WriteLine("인벤토리가 비어있습니다.");
                    }

                    for (int i = 0; i < inventory.Count; i++)
                    {
                        string equippedMarker = "";
                        if (inventory[i] == CurrentWeapon)
                        {
                            equippedMarker = "[E]";
                        }
                        else if (inventory[i] == CurrentArmor)
                        {
                            equippedMarker = "[E]";
                        }
                        Console.WriteLine($"{i + 1}. {equippedMarker} {inventory[i].name} | 타입: {inventory[i].type} | 파워: {inventory[i].power}");
                    }

                    Console.WriteLine("\n9. 나가기"); 

                    Console.Write("\n장착할 아이템의 번호를 선택하세요 (9번: 나가기): ");
                    if (int.TryParse(Console.ReadLine(), out int choice))
                    {
                        if (choice == 9)
                        {
                            Console.WriteLine("장비 관리를 종료합니다.\n");
                            Program.CheckMyState(this);
                        }
                        if (choice > 0 && choice <= inventory.Count)
                        {
                            Item selectedItem = inventory[choice - 1];
                            if (selectedItem.type == ItemType.Weapon)
                            {
                                EquipWeapon(selectedItem); // 무기 장착
                            }
                            else if (selectedItem.type == ItemType.Armor)
                            {
                                EquipArmor(selectedItem); // 방어구 장착
                            }
                            else
                            {
                                Console.WriteLine($"{selectedItem.name}은(는) 장착할 수 없는 아이템입니다.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다. 유효한 번호를 선택해주세요.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다. 숫자를 입력해주세요.");
                    }
                }
            }

            public void AddExp(int addexp)
            {
                int NextLevel = level * 10;
                exp += addexp;
                Console.WriteLine($"{addexp} 경험치를 획득했습니다. 현재 경험치: {exp}/{NextLevel}");
                CheckLevelUp(NextLevel);
            }
            public void CheckLevelUp(int NextLevel)
            {
                if (exp >= NextLevel)
                {
                    exp -= NextLevel;
                    level++;
                    Console.WriteLine($"레벨업! 현재 레벨: {level}");
                }
            }
        }

        //아이템 클레스
        public class Item
        {
            public string name { get; set; }
            public ItemType type { get; set; }
            public int power { get; set; } // 공격력 또는 방어력
            public int value { get; set; } // 아이템의 가치 (골드)

            public Item(string name, ItemType type, int power, int value)
            {
                this.name = name;
                this.type = type;
                this.power = power;
                this.value = value;
            }
        }

        public enum ItemType
        {
            Weapon,
            Armor,
            Potion
        }

        //게임 저장&불러오기
        public class GameDataManager
        {
            private const string filePath = "playerData.json";
            public void SavePlayerData(Player player)
            {
                string jsonData = JsonConvert.SerializeObject(player, Formatting.Indented);
                File.WriteAllText(filePath, jsonData);
                Console.WriteLine("지금까지의 여정을 세이브 했습니다.");
            }

            public Player LoadPlayerData()
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("저장된 세이브 파일이 없습니다.");
                    return null;
                }

                string jsonData = File.ReadAllText(filePath);
                Player player = JsonConvert.DeserializeObject<Player>(jsonData);
                Console.WriteLine("세이브 파일을 불러왔습니다.");
                return player;
            }
        }
    }
}
