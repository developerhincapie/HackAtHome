using System;
namespace HackAtHome.Entities
{
    public enum Status
    {
		Error = 0,
        Success = 1,
        InvalidUserOrNotEvent = 2,
        OutOfDate = 3,
        AllSuccess = 999
    }
}
