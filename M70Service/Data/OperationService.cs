namespace M70Service.Data
{
    // Added as a singleton service to be used by other pages to push operations to the database.
    public class OperationService
    {
        public ApplicationDbContext DbContext { get; set; }

        public Task<Operation> CreateOperation(Job type, Guid branchID, string clientName) {
            Operation op = new Operation(type, Guid.NewGuid(), branchID, clientName);
            return Task.FromResult(op);
        }

        public Task<bool> SubmitOperationToDatabase(Operation op, List<EquipmentUsed> used) {
            bool success = true;
            if (used.Count < 1) success = false;
            else if (op.ClientName == null || op.ClientName.Equals(String.Empty)) success = false;
            else if (op.EndTime < op.StartTime) success = false;
            else if (op.BranchID == Guid.Empty) success = false;
            else if (op.Cost == null || op.Cost < 0f) success = false;

            if (success) {
                DbContext.operationModel.Add(op);
                DbContext.SaveChanges();
                foreach (var item in used) {
                    DbContext.equipmentModel.Add(item);
                }
                DbContext.SaveChanges();
            }


            return Task.FromResult(success);
        }

        public void UpdateJobType(Operation op, Job type) {
            op.JobType = type;
        }

        public void UpdateClient(Operation op, string clientName) { 
            op.ClientName = clientName;
        }

        public void SetTimes(Operation op, DateTime start, DateTime? finish = null) { 
            op.StartTime = start;
            op.EndTime = finish;
        }

        public void SetStatus(Operation op, JobStatus status) {
            op.Status = status;
        }

        public void SetCost(Operation op, float cost) {
            op.Cost = cost;
        }
    }
}
