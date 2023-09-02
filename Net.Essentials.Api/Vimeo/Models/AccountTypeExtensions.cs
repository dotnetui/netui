using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Essentials.Vimeo.Models
{
    public static class AccountTypeExtensions
    {
        public static string ToAccountTypeString(AccountType accountType)
        {
            if (accountType == AccountType.Unknown) return null;
            if (accountType == AccountType.LiveBusiness) return "live_business";
            if (accountType == AccountType.LivePremium) return "live_premium";
            if (accountType == AccountType.LivePro) return "live_pro";
            if (accountType == AccountType.ProUnlimited) return "pro_unlimited";
            return accountType.ToString().ToLowerInvariant();
        }

        public static AccountType ToAccountType(string accountTypeString)
        {
            if (accountTypeString == "advanced") return AccountType.Advanced;
            if (accountTypeString == "basic") return AccountType.Basic;
            if (accountTypeString == "business") return AccountType.Business;
            if (accountTypeString == "enterprise") return AccountType.Enterprise;
            if (accountTypeString == "free") return AccountType.Free;
            if (accountTypeString == "live_business") return AccountType.LiveBusiness;
            if (accountTypeString == "live_premium") return AccountType.LivePremium;
            if (accountTypeString == "live_pro") return AccountType.LivePro;
            if (accountTypeString == "plus") return AccountType.Plus;
            if (accountTypeString == "pro") return AccountType.Pro;
            if (accountTypeString == "pro_unlimited") return AccountType.ProUnlimited;
            if (accountTypeString == "producer") return AccountType.Producer;
            if (accountTypeString == "standard") return AccountType.Standard;
            if (accountTypeString == "starter") return AccountType.Starter;
            return AccountType.Unknown;
        }
    }
}
