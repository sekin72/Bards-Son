using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Zenject;
using System;

public class GameRules : MonoBehaviour
{
    #region Variables
    private UserDataManager _economyManager;
    #endregion
    #region Injection
    [Inject]
    public void Initializor(UserDataManager econ)
    {
        _economyManager = econ;
    }
    #endregion

    #region Stand Max Line Count
    public static IDictionary<ShopType, int> StandMaxLineCount = new Dictionary<ShopType, int>()
                                            {
                                                {ShopType.Drinks, 10},
                                                {ShopType.IceCream, 10},
                                                {ShopType.FastFood, 15},
                                                {ShopType.Cocktail, 10},
                                                {ShopType.SeasideShop, 15},
                                                {ShopType.Sushi, 10},
                                                {ShopType.NightClub, 15},
                                                {ShopType.LuxuryRestaurant, 10},
                                                {ShopType.Hotel, 15}
                                            };
    #endregion
    #region Product Names
    public static IDictionary<int, string> ShopDrinkProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Ice Water"},
                                                {1, "Soda"},
                                                {2, "Cola"},
                                                {3, "Lemonade"},
                                                {4, "Fruit Juice"},
                                                {5, "Mixed Fruit Juice"},
                                                {6, "Coctail"},
                                                {7, "Martini"},
                                                {8, "Big Coctail"},
                                                {9, "Moco Coco"}
                                            };
    public static IDictionary<int, string> ShopIceCreamProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Popstickle"},
                                                {1, "Ice Cream"},
                                                {2, "Fruit Ice Cream"},
                                                {3, "Nougget"},
                                                {4, "Magnum"},
                                                {5, "Cup (1 Spoon)"},
                                                {6, "Cup (3 Spoon)"},
                                                {7, "Frozen Yoghurt"},
                                                {8, "Frozen Shake"},
                                                {9, "Waffle"}
                                            };
    public static IDictionary<int, string> ShopFastFoodProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Chips"},
                                                {1, "French Fries"},
                                                {2, "Nugget"},
                                                {3, "Hot Dog"},
                                                {4, "Taco"},
                                                {5, "Hamburger"},
                                                {6, "Slice Pizza"},
                                                {7, "Fish & Chips"},
                                                {8, "Fried Chicken Box"},
                                                {9, "Mix Combo Plate"}
                                            };
    public static IDictionary<int, string> ShopClubProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Martini"},
                                                {1, "Viski"},
                                                {2, "Kokteyl 1"},
                                                {3, "Bloody Marry"},
                                                {4, "Champaigne (Şişe ve bardak olacak)"},
                                                {5, "Roze Wine (Şişe ve bardak olacak)"},
                                                {6, "Tekila (Şişe ve bardak olacak)"},
                                                {7, "Bourbon (Şişe ve bardak olacak) "},
                                                {8, "Viski (Şişe ve bardak olacak)"},
                                                {9, "Buzlu kovada farklı şişeler"}
                                            };
    public static IDictionary<int, string> ShopDiscoProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Bardakta Bira"},
                                                {1, "Bardakta Siyah Bira"},
                                                {2, "Mojito"},
                                                {3, "Vodka Kokteyl"},
                                                {4, "Tekila Shot / Tequilla"},
                                                {5, "3’lü shot (renkli, tahta servis üzerinde)"},
                                                {6, "Kokteyl 1"},
                                                {7, "Kokteyl 2"},
                                                {8, "Kokteyl 3"},
                                                {9, "Kokteyl 4 (Meyvelerle abartılacak)"}
                                            };
    public static IDictionary<int, string> ShopSeasideProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Makarna"},
                                                {1, "Güneş Kremi"},
                                                {2, "Su Topu"},
                                                {3, "Terlik"},
                                                {4, "Short (Erkek)"},
                                                {5, "Bikini"},
                                                {6, "Güneş Gözlüğü"},
                                                {7, "Su Yatağı"},
                                                {8, "Şişme Flamingo"},
                                                {9, "Sörf Tahtası"}
                                            };
    public static IDictionary<int, string> ShopSushiProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Sushi Roll"},
                                                {1, "Rice Sushi"},
                                                {2, "Noodle"},
                                                {3, "Kyoto California Roll"},
                                                {4, "Baby Shrimp"},
                                                {5, "Somon Wrap"},
                                                {6, "Bao"},
                                                {7, "Sushi Plate"},
                                                {8, "Sushi Mix Plate"},
                                                {9, "Sushi Combo Plate"}
                                            };
    public static IDictionary<int, string> ShopRestaurantProducts = new Dictionary<int, string>()
                                            {
                                                {0, "Midye Tabağı"},
                                                {1, "Kalamar Izgara"},
                                                {2, "Deniz Ürünleri Salatası"},
                                                {3, "Karides Güveç"},
                                                {4, "Ahtapot"},
                                                {5, "Balık Izgara (Levrek?)"},
                                                {6, "Balık Izgara (Somon)"},
                                                {7, "Yengeç"},
                                                {8, "Istakoz"},
                                                {9, "Golden Havyar (Büyük servis tabağının ortasında 3 tane Üzeri büyük yaprakla süslenmiş.) "}
                                            };

    public static IDictionary<ShopType, IDictionary<int, string>> ShopProductNames = new Dictionary<ShopType, IDictionary<int, string>>()
    {
        {ShopType.Drinks, ShopDrinkProducts},
        {ShopType.IceCream, ShopIceCreamProducts},
        {ShopType.FastFood, ShopFastFoodProducts},
        {ShopType.NightClub, ShopClubProducts},
        {ShopType.Cocktail, ShopDiscoProducts},
        {ShopType.SeasideShop, ShopSeasideProducts},
        {ShopType.Sushi, ShopSushiProducts},
        {ShopType.LuxuryRestaurant, ShopRestaurantProducts},
        {ShopType.Hotel, ShopDrinkProducts}
    };

    #endregion
    #region Stand Employee Count
    public static IDictionary<ShopType, int> StandEmployeeCount = new Dictionary<ShopType, int>()
                                            {
                                                {ShopType.Drinks, 1},
                                                {ShopType.IceCream, 0},
                                                {ShopType.FastFood, 0},
                                                {ShopType.Cocktail, 0},
                                                {ShopType.SeasideShop, 0},
                                                {ShopType.Sushi, 0},
                                                {ShopType.NightClub, 0},
                                                {ShopType.LuxuryRestaurant, 0},
                                                {ShopType.Hotel, 0}
                                            };
    #endregion
    #region Level Coefficients
    public static IDictionary<ShopType, double> LevelCoefficients = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 0.2f},
                                                {ShopType.IceCream, 1.8f},
                                                {ShopType.FastFood, 13f},
                                                {ShopType.Cocktail, 70f},
                                                {ShopType.SeasideShop, 4000f},
                                                {ShopType.Sushi, 110000f},
                                                {ShopType.NightClub, 3E+08f},
                                                {ShopType.LuxuryRestaurant, 2E+14f},
                                                {ShopType.Hotel, 3E+17f}
                                            };
    #endregion
    #region Interceptions
    public static IDictionary<ShopType, double> Interception = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 1.8f},
                                                {ShopType.IceCream, 13.2f},
                                                {ShopType.FastFood, 87},
                                                {ShopType.Cocktail, 430f},
                                                {ShopType.SeasideShop, 21000f},
                                                {ShopType.Sushi, 390000f},
                                                {ShopType.NightClub, 6E+08f},
                                                {ShopType.LuxuryRestaurant, 3E+14f},
                                                {ShopType.Hotel, 0f}
                                            };
    #endregion
    #region Multipliers
    private static IDictionary<int, double> _multiplierDrinks = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 20},
                                                {150, 4},
                                                {200, 5},
                                                {300, 6},
                                                {400, 8},
                                                {500, 10},
                                                {750, 600},
                                                {1000, 3000000}
                                            };
    private static IDictionary<int, double> _multiplierIceCream = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 15},
                                                {150, 5},
                                                {200, 5},
                                                {300, 7},
                                                {400, 8},
                                                {500, 10},
                                                {750, 250},
                                                {1000, 2250000}
                                            };
    private static IDictionary<int, double> _multiplierFastFood = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 10},
                                                {150, 5},
                                                {200, 5},
                                                {300, 5},
                                                {400, 10},
                                                {500, 15},
                                                {750, 1350},
                                                {1000, 1440000}
                                            };
    private static IDictionary<int, double> _multiplierCocktail = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 9},
                                                {150, 6},
                                                {200, 6},
                                                {300, 6},
                                                {400, 10},
                                                {500, 20},
                                                {750, 1800},
                                                {1000, 1105000}
                                            };
    private static IDictionary<int, double> _multiplierEquipment = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 8},
                                                {100, 25},
                                                {150, 8},
                                                {200, 8},
                                                {300, 10},
                                                {400, 15},
                                                {500, 15},
                                                {750, 1500},
                                                {1000, 3375000}
                                            };
    private static IDictionary<int, double> _multiplierSushi = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 20},
                                                {150, 8},
                                                {200, 8},
                                                {300, 10},
                                                {400, 20},
                                                {500, 35},
                                                {750, 1750},
                                                {1000, 2160000}
                                            };
    private static IDictionary<int, double> _multiplierNightClub = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {100, 50},
                                                {150, 8},
                                                {200, 10},
                                                {300, 12},
                                                {400, 16},
                                                {500, 22},
                                                {750, 624},
                                                {1000, 420000}
                                            };
    private static IDictionary<int, double> _multiplierLuxuryRestaurant = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 6},
                                                {150, 24},
                                                {200, 8},
                                                {300, 10},
                                                {400, 10},
                                                {500, 12},
                                                {100, 25},
                                                {750, 1350},
                                                {1000, 997500}
                                            };
    private static IDictionary<int, double> _multiplierHotel = new Dictionary<int, double>()
                                            {
                                                {1, 1},
                                                {50, 25},
                                                {150, 42},
                                                {200, 6},
                                                {300, 9},
                                                {400, 16},
                                                {500, 30},
                                                {100, 40},
                                                {750, 3000},
                                                {1000, 937500}
                                            };

    public static IDictionary<ShopType, IDictionary<int, double>> Multipliers = new Dictionary<ShopType, IDictionary<int, double>>()
        {
            {ShopType.Drinks, _multiplierDrinks},
            {ShopType.IceCream, _multiplierIceCream},
            {ShopType.FastFood, _multiplierFastFood},
            {ShopType.Cocktail, _multiplierCocktail},
            {ShopType.SeasideShop, _multiplierEquipment},
            {ShopType.Sushi, _multiplierSushi},
            {ShopType.NightClub, _multiplierNightClub},
            {ShopType.LuxuryRestaurant, _multiplierLuxuryRestaurant},
            {ShopType.Hotel, _multiplierHotel}
        };

    #endregion
    #region Purchase Costs
    public static IDictionary<ShopType, double> PurchaseCosts = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 0d},
                                                {ShopType.IceCream, 10000d},
                                                {ShopType.FastFood, 2500000d},
                                                {ShopType.Cocktail, 1000000000d},
                                                {ShopType.SeasideShop, 1000000000000d},
                                                {ShopType.Sushi, 1000000000000000d},
                                                {ShopType.NightClub, 1000000000000000000d},
                                                {ShopType.LuxuryRestaurant, 1000000000000000000000000d},
                                                {ShopType.Hotel, 1000000000000000000000000000000d}
                                            };
    #endregion
    #region Initial Upgrade Costs
    public static IDictionary<ShopType, double> InitialUpgradeCosts = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 2},
                                                {ShopType.IceCream, 100},
                                                {ShopType.FastFood, 500},
                                                {ShopType.Cocktail, 10000},
                                                {ShopType.SeasideShop, 100000},
                                                {ShopType.Sushi, 20000000},
                                                {ShopType.NightClub, 5*10^9},
                                                {ShopType.LuxuryRestaurant, 4*10^18},
                                                {ShopType.Hotel, 5*10^21}
                                            };
    #endregion
    #region Upgrade Cost Rates
    public static IDictionary<ShopType, double> UpgradeCostRates = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 1.06f},
                                                {ShopType.IceCream, 1.06f},
                                                {ShopType.FastFood, 1.07f},
                                                {ShopType.Cocktail, 1.07f},
                                                {ShopType.SeasideShop, 1.07f},
                                                {ShopType.Sushi, 1.07f},
                                                {ShopType.NightClub, 1.07f},
                                                {ShopType.LuxuryRestaurant,1.08f},
                                                {ShopType.Hotel, 1.08f}
                                            };
    #endregion
    #region Hire Employee Costs
    public static IDictionary<ShopType, double> HireEmployeeCost1 = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 1000},
                                                {ShopType.IceCream, 50000},
                                                {ShopType.FastFood, 5000000},
                                                {ShopType.Cocktail, 5*10^6},
                                                {ShopType.SeasideShop, 5*10^12},
                                                {ShopType.Sushi, 25*10^12},
                                                {ShopType.NightClub, 9*10^18},
                                                {ShopType.LuxuryRestaurant, 25*10^24},
                                                {ShopType.Hotel, 100*10^30}
                                            };
    public static IDictionary<ShopType, double> HireEmployeeCost2 = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 500000},
                                                {ShopType.IceCream, 100000000},
                                                {ShopType.FastFood, 250000000},
                                                {ShopType.Cocktail, 5000000000},
                                                {ShopType.SeasideShop, 5000000000000000000},
                                                {ShopType.Sushi, 62500000000000000},
                                                {ShopType.NightClub, 3.06E+22},
                                                {ShopType.LuxuryRestaurant, 8.75E+28},
                                                {ShopType.Hotel, 6E+35}
                                            };
    public static IDictionary<ShopType, double> HireEmployeeCost3 = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 250000000},
                                                {ShopType.IceCream, 200000000000},
                                                {ShopType.FastFood, 25000000000},
                                                {ShopType.Cocktail, 5000000000000},
                                                {ShopType.SeasideShop, 5E+21},
                                                {ShopType.Sushi, 1.5625E+20},
                                                {ShopType.NightClub, 1.0404E+26},
                                                {ShopType.LuxuryRestaurant, 3.06E+32},
                                                {ShopType.Hotel, 1E+41}
                                            };
    public static IDictionary<ShopType, double> HireEmployeeCost4 = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 125000000000},
                                                {ShopType.IceCream, 25000000000000},
                                                {ShopType.FastFood, 50000000000},
                                                {ShopType.Cocktail, 5000000000000000},
                                                {ShopType.SeasideShop, 5E+24},
                                                {ShopType.Sushi, 3.90625E+23},
                                                {ShopType.NightClub, 3.53736E+29},
                                                {ShopType.LuxuryRestaurant, 1.07E+36},
                                                {ShopType.Hotel, 2.16E+43}
                                            };
    public static IDictionary<int, IDictionary<ShopType, double>> HireEmployeeCost = new Dictionary<int, IDictionary<ShopType, double>>()
        {
            {2, HireEmployeeCost1},
            {3, HireEmployeeCost2},
            {4, HireEmployeeCost3},
            {5, HireEmployeeCost4}
        };

    #endregion
    #region Initial Salary Cost
    public static IDictionary<ShopType, double> InitialSalaryCosts = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 100},
                                                {ShopType.IceCream, 1000},
                                                {ShopType.FastFood, 10000},
                                                {ShopType.Cocktail, 100000},
                                                {ShopType.SeasideShop, 1000000},
                                                {ShopType.Sushi, 10000000},
                                                {ShopType.NightClub, 800*10^6},
                                                {ShopType.LuxuryRestaurant,8*10^15},
                                                {ShopType.Hotel, 500*10^21}
                                            };
    #endregion
    #region Salary Cost Rates
    public static IDictionary<ShopType, double> SalaryCostRates = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 1.80f},
                                                {ShopType.IceCream, 1.80f},
                                                {ShopType.FastFood, 1.80f},
                                                {ShopType.Cocktail, 1.80f},
                                                {ShopType.SeasideShop, 1.80f},
                                                {ShopType.Sushi, 1.90f},
                                                {ShopType.NightClub, 2f},
                                                {ShopType.LuxuryRestaurant, 2f},
                                                {ShopType.Hotel, 1.95f}
                                            };
    #endregion
    #region Initial Processing Times
    public static IDictionary<ShopType, double> InitialProcessingTimes = new Dictionary<ShopType, double>()
                                            {
                                                {ShopType.Drinks, 2},
                                                {ShopType.IceCream, 3},
                                                {ShopType.FastFood, 8},
                                                {ShopType.Cocktail, 10},
                                                {ShopType.SeasideShop, 25},
                                                {ShopType.Sushi, 120},
                                                {ShopType.NightClub, 600},
                                                {ShopType.LuxuryRestaurant,1800},
                                                {ShopType.Hotel, 7200}
                                            };
    #endregion
    #region Product Availability Levels
    private static IDictionary<int, int> ProductAvailabilityLevels = new Dictionary<int, int>()
                                            {
                                                {1, 0},
                                                {50, 1},
                                                {100, 2},
                                                {150, 3},
                                                {200, 4},
                                                {300, 5},
                                                {400, 6},
                                                {500, 7},
                                                {750, 8},
                                                {1000, 9}
                                            };
    #endregion
    #region Parking Lot Upgrade Cost
    private static IDictionary<int, double> ParkingLotUpgradeCosts = new Dictionary<int, double>()
                                            {
                                                {1, 50},
                                                {2, 1000},
                                                {3, 10000},
                                                {4, 150000},
                                                {5, 1000000},
                                                {6, 10000000},
                                                {7, 250000000},
                                                {8, 100000000000},
                                                {9, 10000000000000},
                                                {10, 100000000000000},
                                                {11, 1000000000000000},
                                                {12, 25000000000000000},
                                                {13, 500000000000000000},
                                                {14, 975000000000000000},
                                                {15, 1450000000000000000},
                                                {16, 1925000000000000000},
                                                {17, 2400000000000000000},
                                                {18, 2875000000000000000},
                                                {19, 3.35E+22},
                                                {20, 3.825E+27},
                                                {21, 4.3E+29},
                                                {22, 4.775E+35}
                                            };
    #endregion

    #region Methods
    private static double GetMultiplier(int level, ShopType type)
    {
        var tempDic = Multipliers.FirstOrDefault(x => x.Key == type).Value;

        var i = 0;
        var multiplier = 1d;

        while (level >= tempDic.Keys.ElementAt(i))
        {
            var x = tempDic.Values.ElementAt(i);
            multiplier *= x;
            i++;
            if (i == tempDic.Values.Count)
                break;
        }
        return multiplier;
    }
    public static double GetUpgradeShopCost(ShopType type, int level)
    {
        return level * UpgradeCostRates[type];
    }
    public static double GetProcessingTime(ShopType type, double salaryLevel)
    {
        return ((100 * InitialProcessingTimes[type]) / (100 + salaryLevel * 5) * 100);
    }
    public static double GetHireCost(ShopType type, int employeeCount)
    {
        return HireEmployeeCost[employeeCount][type];
    }
    public static double CustomerServed(int numOfEmp, double processTime)
    {
        return 60 / processTime * numOfEmp;
    }
    public static double Earnings(ShopType type, int numOfEmp, double processTime, int level)
    {
        return TPV(type, level) * CustomerServed(numOfEmp, processTime);
    }
    public static double GetSalaryCost(ShopType type, double currentSalaryLevel)
    {
        return currentSalaryLevel * SalaryCostRates[type];
    }
    public static double TPV(ShopType type, int level)
    {
        return (LevelCoefficients[type] * level + Interception[type]) * GetMultiplier(level, type);
    }
    public static int GetAvailableProduct(int level)
    {
        return ProductAvailabilityLevels.Last(x => level >= x.Key).Value;
    }
    public static int ParkingLotAdvertisingCost(int advertisementLevel)
    {
        return 2 * 2 ^ advertisementLevel;
    }
    public static double ParkingLotCarsEntry(int advertisementLevel)
    {
        return 0.8 * advertisementLevel + 7.2;
    }
    public static double GetParkingLotUpgradeCost(int newLevel)
    {
        return ParkingLotUpgradeCosts[newLevel];
    }
    #endregion
}