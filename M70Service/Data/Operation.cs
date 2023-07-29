namespace M70Service.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    // File contains all data structures to record operational history

    public enum Equipment { 
        // Vechicles
        CRANE, TRUCK,

        // Servicing & Repair
        WELDER, PLASMA_CUTTER, TOOLBOX, PAINT_TOOLS, REPLACEMENT_PARTS,

        // Software
        SOFTWARE_LICENCE, UPDATE, COMPUTE,

        // Safety inspection
        FIRST_AID, HIGH_VIS, HARDHAT
    }

    public enum Job
    {
        SERVICE, SOFTWARE, REPAIR, INSPECTION
    }

    public enum JobStatus
    {
        PENDING, IN_PROGRESS, EXPECTED_COMPLETED, COMPLETED
    }

    public class Branches
    {
        public static KeyValuePair<Guid, string> branch1 = new KeyValuePair<Guid, string>(Guid.NewGuid(), "Branch1");
        public static KeyValuePair<Guid, string> branch2 = new KeyValuePair<Guid, string>(Guid.NewGuid(), "Branch2");
    }

    public class EquipmentUsed
    {
        [Key] public Guid EquipmentID { get; set; }

        [Required(ErrorMessage = "Specify type of equipment")]
        public Equipment? Equipment { get; set; }

        public Guid JobID { get; set; }

        [Required(ErrorMessage = "Amount of equipment used is required")]
        public float? Amount { get; set; }

        public EquipmentUsed() { }
    }

    /* Data structrure to be stored in database and contains information relating to that job.*/
    public class Operation {
        [Key] public Guid JobID { get; set; }
        public Job JobType { get; set; }

        public Guid BranchID { get; set; }
        public string ClientName { get; set; }

        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public JobStatus Status { get; set; }
        public float? Cost { get; set; }

        public Operation() {
            JobID = Guid.Empty;
            BranchID = Guid.Empty;
            ClientName = String.Empty;
            StartTime = null;
            EndTime = null;
            Cost = null;
        }

        public Operation(Guid jobID, Guid branchID, string clientName) {
            JobID = jobID;
            BranchID = branchID;
            ClientName = clientName;
        }

        public Operation(Job jobType, Guid jobID, Guid branchID, string clientName) {
            JobType = jobType;

            JobID = jobID;
            BranchID = branchID;
            ClientName = clientName;

            Status = JobStatus.PENDING;
        }
    }


}