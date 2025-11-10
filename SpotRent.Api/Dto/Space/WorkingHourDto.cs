namespace SpotRent.Api.Dto.Space;

public record WorkingHourDto(int DayOfWeek, string OpenTime, string CloseTime, bool IsAvailable = true);
