using System.ComponentModel;

namespace Dropbox.Enum
{
    public enum AccountType
    {
        [Description("Basic")]
        Basic = 1,

        [Description("Business")]
        Business = 2,

        [Description("Professional")]
        Pro = 3
    }
}
