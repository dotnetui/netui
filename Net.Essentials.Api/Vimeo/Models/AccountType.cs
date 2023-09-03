using Net.Essentials.Vimeo.JsonConverters;

using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public enum AccountType
    {
        Unknown,
        Advanced,
        Basic,
        Business,
        Enterprise,
        Free,
        LiveBusiness,
        LivePremium,
        LivePro,
        Plus,
        Pro,
        ProUnlimited,
        Producer,
        Standard,
        Starter
    }

    public class AccountTypeEnumBinding : StringEnumBinding<AccountType>
    {
        public static AccountTypeEnumBinding Instance { get; } = new AccountTypeEnumBinding();

        protected override Dictionary<string, AccountType> Populate() => new Dictionary<string, AccountType>
        {
            { "live_business", AccountType.LiveBusiness },
            { "live_premium", AccountType.LivePremium },
            { "live_pro", AccountType.LivePro },
            { "" }
        };
    }
}
