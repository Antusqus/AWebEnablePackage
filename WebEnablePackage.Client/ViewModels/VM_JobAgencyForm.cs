using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebEnablePackage.ViewModels.JobAgency
{

    public class VM_JobAgency
    {

        public GenderOptions Gender { get; set; }
            public enum GenderOptions: int
            {
                [Display(Name = "Prefer not to say")]
                PreferNotToSay = 0, Male = 1, Female = 2, Other = 3
            }
        public string TeleNr { get; set; }
        public string Place { get; set; }
        public FieldOfWorkOptions FieldOfWork { get; set; }
            public enum FieldOfWorkOptions: int
            {
                Office = 0, Engineering = 1, Hospitality = 2, Other = 3
            }
        public string Education { get; set; }
        public string Experience { get; set; }
        public string DesiredFunction { get; set; }
        public MotivationOptions Motivation { get; set; }

            public enum MotivationOptions: int
            {
                [Display(Name = "Looking for work")]

                LookingForWork = 0,
                [Display(Name = "Looking for new challenges")]
                NewChallenge = 1,
                [Display(Name = "Changing up the workload")]
                ChangeInWorkload = 2,
                [Display(Name = "Looking closer to home")]
                CloserToHome = 3,
                Other = 4
            }

        public LocationOptions Location { get; set; }

        public enum LocationOptions : int
        {
            Zwaag = 0, Heerhugowaard = 1
        }
        
        public string Comments { get; set; }
    }
}