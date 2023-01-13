using System.ComponentModel.DataAnnotations;

namespace CoderThoughtsBlog.Enums
{
    public enum ReadyStatus
    {
        Incomplete,
        [Display(Name ="Complete/Ready")]
        ProductionReady,
        [Display(Name = "Ready to Test")]
        PreviewReady
    }
}
