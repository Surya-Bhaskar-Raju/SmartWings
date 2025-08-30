using Microsoft.AspNetCore.Mvc;
using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartWings.Application.Contracts;

namespace SmartWings.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing notifications.
    /// Provides endpoints to retrieve notifications for a specific user.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service dependency.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>
        /// A list of notifications for the given user if found; 
        /// otherwise, returns a NotFound response.
        /// </returns>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(Guid userId)
        {
            // Call the service layer to fetch notifications by user ID
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);

            // If no notifications are found, return a 404 Not Found response
            if (notifications == null)
                return NotFound($"No notifications found for user ID: {userId}");

            // Return 200 OK with the list of notifications
            return Ok(notifications);
        }
    }
}
