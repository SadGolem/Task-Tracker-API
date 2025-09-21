using System.Security.Claims;
using Task_Tracker_API.domain;

namespace Task_Tracker_API;

public static class ClaimsExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal me)
    {
        var id = me.FindFirstValue(ClaimTypes.NameIdentifier) ?? me.FindFirstValue(ClaimTypes.NameIdentifier) ?? me.FindFirstValue(ClaimTypes.Name);
        return Guid.TryParse(id, out var g) ? g : null;
    }


    public static bool IsAdmin(this ClaimsPrincipal me) => me.IsInRole("Admin");


    public static bool CanEdit(this ClaimsPrincipal me, TaskItem e)
    {
        var id = me.GetUserId();
        return me.IsAdmin() || (id is Guid g && (e.AuthorId == g || e.AssigneeId == g));
    }
}