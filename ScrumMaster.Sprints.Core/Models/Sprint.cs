namespace ScrumMaster.Sprints.Core.Models
{
    public class Sprint
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string CreatedBy { get; private set; }
        public Guid CreatedUserId { get; private set; }
        public Guid ProjectId { get; private set; }
        public bool IsActual { get; private set; }
        private Sprint() { }
        public Sprint(string name,DateTime startDate, DateTime endDate, string createdBy, Guid createdUserId,Guid projectId)
        {
            Id = Guid.NewGuid();
            SetName(name);
            SetSprintDate(startDate,endDate);
            SetCreatedBy(createdBy);
            SetCreatedUserId(createdUserId);
            IsActual = true;
            ProjectId = projectId;
        }
        public void SetActual(bool isActual)
        {
            IsActual = isActual;
        }
        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new Exception("Name_Cannot_Be_Empty_Or_Null");
            Name = name;
        }
        public void SetSprintDate(DateTime startDate, DateTime endDate)
        {
            if (DateOnly.FromDateTime(startDate) > DateOnly.FromDateTime(endDate))
                throw new Exception("StartDate_Cannot_Be_After_EndDate");
            if (DateOnly.FromDateTime(startDate) == DateOnly.FromDateTime(endDate))
                throw new Exception("StartDate_Cannot_Be_The_Same_As_EndDate");
            StartDate = startDate;
            EndDate = endDate;
        }
        public void SetCreatedBy(string createdBy)
        {
            if (string.IsNullOrEmpty(createdBy))
                throw new Exception("CreatedBy_Cannot_Be_Null_Or_Empty");
            CreatedBy = createdBy;
        }
        public void SetCreatedUserId(Guid createdUserId)
        {
            if (createdUserId == Guid.Empty)
                throw new Exception("CreatedUserId_Cannot_Be_Empty");
            CreatedUserId = createdUserId;
        }
        public bool UpdateSprint(string name, string createdBy, DateTime startDate, DateTime endDate)
        {
            var AnyChange = false;
            if (!string.IsNullOrWhiteSpace(name) && Name != name)
            {
                Name = name;
                AnyChange = true;
            }
            if(!string.IsNullOrWhiteSpace(createdBy) && CreatedBy != createdBy)
            {
                CreatedBy = createdBy;
                AnyChange = true;
            }
            if (startDate != StartDate && startDate != DateTime.MinValue)
            {
                if (endDate != EndDate && endDate != DateTime.MinValue)
                {
                    if (startDate >= endDate)
                        throw new Exception("StartDate_Cannot_Be_The_Same_Or_After_As_EndDate");
                }
                else
                {
                    if (startDate >= EndDate)
                        throw new Exception("StartDate_Cannot_Be_The_Same_Or_After_As_EndDate");
                }
                
                StartDate = startDate;

                AnyChange = true;
            }
            if (endDate != EndDate && endDate != DateTime.MinValue)
            {
                if (startDate != StartDate && StartDate != DateTime.MinValue)
                {
                    if (startDate >= endDate)
                        throw new Exception("EndDate_Cannot_Be_The_Same_Or_Befor_As_StartDate");
                }
                else
                {
                    if (StartDate >= endDate)
                        throw new Exception("EndDate_Cannot_Be_The_Same_Or_After_As_StartDate");
                }
                EndDate = endDate;
                AnyChange = true;
            }           
            return AnyChange;            
        }
    }
}
