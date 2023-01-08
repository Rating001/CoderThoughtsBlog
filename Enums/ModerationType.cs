using System.ComponentModel;

namespace CoderThoughtsBlog.Enums
{
    public enum ModerationType
    {
        [Description("Offensive Language")]
        Language,
        [Description("Off Topic")]
        OffTopic,
        [Description("Political Issue")]
        Political,
        [Description("Drug or Alcohol Reference")]
        DrugsAlcohol,
        [Description("Threatening Content")]
        Threatening,
        [Description("Sexual Content")]
        Sexual,
        [Description("Hate Speech")]
        HateSpeech,
        [Description("Targeted Shaming")]
        Shaming,
        [Description("Other Issue - See Notes")]
        Other
    }
}
