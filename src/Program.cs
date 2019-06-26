using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;

namespace PlayFabAuthentication
{
    public static class Program
    {
        static bool _loginSuccessful = false;

        static void Main(string[] args)
        {
            PlayFabSettings.staticSettings.TitleId = "9BB50";

            if (args.Length == 0)
            {
                PlayFabClientAPI.LoginWithCustomIDAsync(new LoginWithCustomIDRequest { CustomId = "UniqueValueForThisDevice", CreateAccount = true })
                                .ContinueWith(OnLoginComplete)
                                .Wait();

                if (_loginSuccessful)
                {
                    var data = new Dictionary<string, string>();
                    data.Add("Points", "100");
                    data.Add("Region", "Somewhere in the Pacific Ocean");

                    PlayFabClientAPI.UpdateUserDataAsync(new UpdateUserDataRequest { Data = data })
                                    .ContinueWith(OnUpdateUserDataComplete)
                                    .Wait();

                PlayFabClientAPI.AddUsernamePasswordAsync(new AddUsernamePasswordRequest
                                        {
                                            Username = "SummerSquash",
                                            Email = "chilberto@wherever.com",
                                            Password = "TickityB00!"
                                        })
                                .ContinueWith(OnAddUsernamePasswordCompleted)
                                .Wait();
                }
            }
            else
            {
                PlayFabClientAPI.LoginWithEmailAddressAsync(new LoginWithEmailAddressRequest { Email = args[0], Password = args[1] })
                                .ContinueWith(OnLoginComplete)
                                .Wait();
            }
                                  
            Console.WriteLine("Done! Press any key to close");
            Console.ReadKey(); // This halts the program and waits for the user
        }

        private static void OnLoginComplete(Task<PlayFabResult<LoginResult>> taskResult)
        {
            if (taskResult.Result.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red; 
                Console.WriteLine("Failure calling PlayFab Authentication");                
                Console.WriteLine(PlayFabUtil.GenerateErrorReport(taskResult.Result.Error));
                Console.ForegroundColor = ConsoleColor.Gray; 
            }
            else 
            if (taskResult.Result.Result != null)
            {
                Console.WriteLine("Congratulations, you made your first successful API call!");
                _loginSuccessful = true;
            }            
        }

        private static void OnUpdateUserDataComplete(Task<PlayFabResult<UpdateUserDataResult>> taskResult)
        {
            if (taskResult.Result.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failure calling PlayFab UpdateUserData");
                Console.WriteLine(PlayFabUtil.GenerateErrorReport(taskResult.Result.Error));
                Console.ForegroundColor = ConsoleColor.Gray;
            }            
        }

        private static void OnAddOrUpdateContactEmailComplete(Task<PlayFabResult<AddOrUpdateContactEmailResult>> taskResult)
        {
            if (taskResult.Result.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failure calling PlayFab AddOrUpdateContactEmail");
                Console.WriteLine(PlayFabUtil.GenerateErrorReport(taskResult.Result.Error));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        private static void OnAddUsernamePasswordCompleted(Task<PlayFabResult<AddUsernamePasswordResult>> taskResult)
        {
            if (taskResult.Result.Error != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failure calling PlayFab AddUsernamePassword");
                Console.WriteLine(PlayFabUtil.GenerateErrorReport(taskResult.Result.Error));
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}