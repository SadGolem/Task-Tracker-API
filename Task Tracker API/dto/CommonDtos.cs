namespace Task_Tracker_API.dto;

public class CommonDtos
{
    public record PagedResult<T>(IReadOnlyList<T> Items, int Total, int Page, int PageSize);
}