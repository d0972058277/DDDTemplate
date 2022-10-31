using Architecture;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Project.Application.Queries.ListNotifications;
using Project.Application.Queries.ListNotifications.Models;

namespace Project.Infrastructure.Queries.ListNotifications
{
    public class ListNotificationsQueryHandler : IQueryHandler<ListNotificationsQuery, IReadOnlyList<Notification>>
    {
        private readonly ReadonlyProjectDbContext _dbContext;

        public ListNotificationsQueryHandler(ReadonlyProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Notification>> Handle(ListNotificationsQuery request, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
Notification.Id,
Notification.Message_Title AS Title,
Notification.Message_Body AS Body,
Notification.Schedule_Value AS Schedule,
Notification.PushedTime,
DeviceNotification.ReadTime
FROM Project.Device
JOIN Project.DeviceNotification ON DeviceNotification.DeviceId = Device.Id
JOIN Project.Notification ON Notification.Id = DeviceNotification.Id
WHERE TRUE
AND Device.Id = @deviceId;";

            var datas = await _dbContext.Database.GetDbConnection().QueryAsync<
                (
                    Guid id,
                    string title,
                    string body,
                    DateTime schedule,
                    DateTime? pushedTime,
                    DateTime? readTime
                )>(sql, new { deviceId = request.DeviceId });

            var notifications = datas.Select(data =>
                new Notification
                (
                    data.id,
                    new Notification.MessageDto
                    (
                        data.title,
                        data.body
                    ),
                    data.schedule,
                    data.pushedTime,
                    data.readTime
                )).ToList();

            return notifications;
        }
    }
}