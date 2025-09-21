using Microsoft.AspNetCore.Authorization;

namespace Task_Tracker_API.auth;

public static class Policies
{
    public const string Admin = "Admin";


    public static void AddPolicies(AuthorizationOptions options)
    {
        options.AddPolicy(Admin, p => p.RequireRole("Admin"));
    }
}