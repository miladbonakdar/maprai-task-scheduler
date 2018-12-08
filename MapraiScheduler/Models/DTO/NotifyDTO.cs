using MapraiScheduler.Models.Database;
using MapraiScheduler.Notifier;

namespace MapraiScheduler.Models.DTO
{
    public class NotifyDTO
    {
        public NotifySetting.Priority Priority { get; set; }
        public long? ProjectID { get; set; }
        public long? NotifyID { get; set; }
        public long NotifyTypeID { get; set; }
        public long? UserID { get; set; }
        public long? PhoneID { get; set; }
        public long? OrganizationID { get; set; }
        public ushort Seen { get; set; }
        public string ToEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string NotifyColor { get; set; }
        public string PriorityName { set; get; }
        public string EventDescription { set; get; }
        public string PersianDateTime { set; get; }
        public string ProjectDetail { set; get; }
        public string ProjectDetailUrl { set; get; }
        public string ProjectPhoneDetail { set; get; }
        public string ProjectPhoneDetailUrl { set; get; }
        public string ProjectAdminDetail { set; get; }

        public void MergeWithNotifyType(NotifyType notifyType)
        {
            NotifyColor = notifyType.NotifyColor;
            PriorityName = notifyType.Priority.GetDescription();
            NotifyTypeID = notifyType.NotifyTypeID;
            EventDescription = notifyType.Description;
        }
    }
}